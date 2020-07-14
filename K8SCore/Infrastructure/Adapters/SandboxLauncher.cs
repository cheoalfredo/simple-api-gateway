using k8s;
using k8s.Models;
using K8SCore.Domain;
using K8SCore.Domain.Ports;
using K8SCore.Infrastructure.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace K8SCore.Infrastructure.Adapters
{
    public class SandboxLauncher : ISandboxLauncher
    {
        private readonly IKubernetes _k8s;
        private readonly ILogger<SandboxLauncher> _logger;

        public SandboxLauncher(IKubernetes k8s, ILogger<SandboxLauncher> logger)
        {
            _k8s = k8s;
            _logger = logger;
        }

        public async Task<InstanceDetails> Create(Dictionary<string, string> labels)
        {
            var deploymentIdentifier = Guid.NewGuid().ToString().ToLower().Replace("-", string.Empty);

            var password = (deploymentIdentifier + DateTime.Now.ToString()).MD5Hash().Replace("-", string.Empty);

            var depl = new V1Deployment
            {
                ApiVersion = "apps/v1",
                Kind = "Deployment",
                Metadata = new V1ObjectMeta
                {
                    Name = $"vscode{deploymentIdentifier}",
                    NamespaceProperty = "sandbox",
                    Labels = new Dictionary<string, string>
                    {
                        { "app", $"vscode{deploymentIdentifier}" }
                    }
                    .Concat(labels).ToDictionary(x => x.Key, x => x.Value)
                },
                Spec = new V1DeploymentSpec
                {
                    Replicas = 1,
                    Selector = new V1LabelSelector
                    {
                        MatchLabels = new Dictionary<string, string>
                         {
                             {"app", $"vscode{deploymentIdentifier}"}
                         }
                    },
                    Template = new V1PodTemplateSpec
                    {
                        Metadata = new V1ObjectMeta
                        {
                            Labels = new Dictionary<string, string>
                            {
                                {"app", $"vscode{deploymentIdentifier}"}
                            }
                        },
                        Spec = new V1PodSpec
                        {
                            Containers = new List<V1Container>
                                {
                                     new V1Container {
                                         Image = "lekamor2006/vscode-nubloq:20200708",                                         
                                         Name = $"vscode{deploymentIdentifier}",
                                         Ports = new List<V1ContainerPort>
                                         {
                                             new V1ContainerPort(8080)
                                         },
                                         Env = new List<V1EnvVar>
                                         {
                                             new V1EnvVar("PASSWORD", password)
                                         }
                                     }
                                }
                        }
                    }
                }
            };

            var svc = new V1Service
            {
                ApiVersion = "v1",
                Kind = "Service",
                Metadata = new V1ObjectMeta { Name = $"vscode{deploymentIdentifier}" },
                Spec = new V1ServiceSpec
                {
                    Selector = new Dictionary<string, string>
                    {
                        {"app", $"vscode{deploymentIdentifier}" }
                    },
                    Ports = new List<V1ServicePort>
                    {
                        new V1ServicePort
                        {
                            Protocol = "TCP",
                            Port = 80,
                            TargetPort = 8080
                        }
                    }
                }

            };

            var ing = new Networkingv1beta1Ingress
            {
                ApiVersion = "networking.k8s.io/v1beta1",
                Kind = "Ingress",
                Metadata = new V1ObjectMeta
                {
                    Name = $"vscode{deploymentIdentifier}",
                    Annotations = new Dictionary<string, string> {
                        {"nginx.ingress.kubernetes.io/rewrite-target","/$2" },
                        {"nginx.ingress.kubernetes.io/app-root", $"/{deploymentIdentifier}" },
                        {"kubernetes.io/ingress.class", "nginx" },
                        {"nginx.ingress.kubernetes.io/proxy-read-timeout", "3600" },
                        {"nginx.ingress.kubernetes.io/proxy-send-timeout","3600" },                       
                        {"cert-manager.io/cluster-issuer", "letsencrypt-prod" }
                    }
                },
                Spec = new Networkingv1beta1IngressSpec
                {
                    Tls = new List<Networkingv1beta1IngressTLS> {
                        new Networkingv1beta1IngressTLS
                        {
                            SecretName = "nubloq-sandbox",
                            Hosts = new List<string>
                            {
                                "nubloq-sandbox.eastus2.cloudapp.azure.com"
                            }
                        }
                    },
                    Rules = new List<Networkingv1beta1IngressRule>
                      { 
                          new Networkingv1beta1IngressRule
                          {
                              Host = "nubloq-sandbox.eastus2.cloudapp.azure.com",
                               Http = new Networkingv1beta1HTTPIngressRuleValue
                               {
                                    Paths = new List<Networkingv1beta1HTTPIngressPath>
                                    {
                                     new Networkingv1beta1HTTPIngressPath
                                     {
                                          Path = $"/{deploymentIdentifier}(/|$)(.*)",
                                          Backend = new Networkingv1beta1IngressBackend
                                          {
                                               ServiceName = $"vscode{deploymentIdentifier}",
                                               ServicePort = 80
                                          }
                                     }
                                    }
                               }
                          }
                      }
                }
            };

            try
            {
                await _k8s.CreateNamespacedDeploymentAsync(depl, "sandbox");
                await _k8s.CreateNamespacedServiceAsync(svc, "sandbox");
                await _k8s.CreateNamespacedIngress1Async(ing, "sandbox");
                _logger.LogInformation($"instance {deploymentIdentifier} created");
                return new InstanceDetails
                {
                    DeploymentIdentifier = deploymentIdentifier,
                    Password = password
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }

        public async Task Destroy(string deploymentIdentifier)
        {

            try
            {
                await _k8s.DeleteNamespacedIngress1Async($"vscode{deploymentIdentifier}", "sandbox");
                await _k8s.DeleteNamespacedServiceAsync($"vscode{deploymentIdentifier}", "sandbox");
                await _k8s.DeleteNamespacedDeploymentAsync($"vscode{deploymentIdentifier}", "sandbox");
                _logger.LogInformation($"instance {deploymentIdentifier} deleted");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }


    }
}
