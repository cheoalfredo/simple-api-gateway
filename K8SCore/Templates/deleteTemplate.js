const axios = require('axios').default;

axios.delete('_url').then((res) => {
    console.log(`statusCode: ${res.status}`)
    console.log(res)
}).catch((error) => {
    console.error(error)
})