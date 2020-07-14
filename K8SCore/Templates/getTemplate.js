const axios = require('axios').default;

axios.get('_url').then((res) => {
    console.log(`statusCode: ${res.status}`)
    console.log(res)
}).catch((error) => {
    console.error(error)
})