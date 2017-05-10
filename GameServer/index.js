const dgram = require('dgram')
const server = dgram.createSocket('udp4')

server.on('error', (err) => {
    console.log(`server error:\n${err.stack}`)
    server.close()
})

server.on('message', (msg, rinfo) => {
    console.log(`server got: ${msg} from ${rinfo.address}:${rinfo.port}`)

    const message = Buffer.from(msg)
    const client = dgram.createSocket('udp4')
    client.send(message, 45998, 'localhost', (err) => {
        if (err) throw err
        console.log('sent! ')
        client.close()
    })
    const client2 = dgram.createSocket('udp4')
    client2.send(message, 45999, 'localhost', (err) => {
        if (err) throw err
        console.log('sent! ')
        client2.close()
    })

})

server.on('listening', () => {
    var address = server.address()
    console.log(`server listening ${address.address}:${address.port}`)
})

server.bind(20547)
