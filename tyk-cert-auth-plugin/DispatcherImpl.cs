using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Coprocess;


namespace tyk_cert_auth_plugin
{
    class DispatcherImpl : Dispatcher.DispatcherBase
    {
        public DispatcherImpl()
        {
            Console.WriteLine("Instantiating DispatcherImpl");
        }


        // The Dispatch method will be called by Tyk for every configured hook, we'll implement a very simple dispatcher here:
        public override Task<Coprocess.Object> Dispatch(Coprocess.Object thisObject, ServerCallContext context)
        {
            // thisObject is the request object:
            Console.WriteLine("Receiving object: " + thisObject.ToString());

            // hook contains the hook name, this will be defined in our plugin bundle and the implementation will be a method in this class (DispatcherImpl), we'll look it up:
            var hook = this.GetType().GetMethod(thisObject.HookName);

            // If hook is null then a handler method for this hook isn't implemented, we'll log this anyway:
            if (hook == null)
            {
                Console.WriteLine("Hook name: " + thisObject.HookName + " (not implemented!)");
                // We return the unmodified request object, so that Tyk can proxy this in the normal way.
                return Task.FromResult(thisObject);
            };

            // If there's a handler method, let's log it and proceed with our dispatch work:
            Console.WriteLine("Hook name: " + thisObject.HookName + " (implemented)");

            // This will dynamically invoke our hook method, and cast the returned object to the required Protocol Buffers data structure:
            var output = hook.Invoke(this, new object[] { thisObject, context });
            return (Task<Coprocess.Object>)output;
        }

        // MyPreMiddleware implements a PRE hook, it will be called before the request is proxied upstream and before the authentication step:
        public Task<Coprocess.Object> MyPreMiddleware(Coprocess.Object thisObject, ServerCallContext context)
        {
            Console.WriteLine("Calling MyPreMiddleware.");
            // We'll inject a header in this request:
            thisObject.Request.SetHeaders["my-header"] = "my-value";
            return Task.FromResult(thisObject);
        }

        // MyAuthCheck implements a custom authentication mechanism, it will initialize a session object if the token matches a certain value:
        public Task<Coprocess.Object> MyAuthMiddleware(Coprocess.Object thisObject, ServerCallContext context)
        {
            // Request.Headers contains all the request headers, we retrieve the authorization token:
            var token = thisObject.Request.Headers["Authorization"];
            Console.WriteLine("Calling MyAuthCheck with token = " + token);

            // We initialize a session object if the token matches "abc123":
            //if (token == "abc123")
            //{
                Console.WriteLine("Successful auth!");
                var session = new Coprocess.SessionState();
                session.Rate = 1000;
                session.Per = 10;
                session.QuotaMax = 60;
                session.QuotaRenews = 1479033599;
                session.QuotaRemaining = 0;
                session.QuotaRenewalRate = 120;
                session.Expires = 1479033599;

                session.LastUpdated = 1478033599.ToString();

                thisObject.Metadata["token"] = token;
                thisObject.Session = session;
                return Task.FromResult(thisObject);

            //}

            // If the token isn't "abc123", we return the request object in the original state, without a session object, Tyk will reject this request:
            /*Console.WriteLine("Rejecting auth!");
            return Task.FromResult(thisObject);*/
        }
    }
}
