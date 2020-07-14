const axios = require('axios').default;

axios.post('_url', {
    _data
}).then((res) => {
    console.log(`statusCode: ${res.status}`)
    console.log(res)
}).catch((error) => {
    console.error(error)
})