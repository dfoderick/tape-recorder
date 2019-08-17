
// this app will read a tape (tape.txt) and put it on chain

const axios = require('axios');
const bsv = require('bsv')
const fs = require('fs')

const walletInfo = require(`./wallet.json`)

function getPrivateKey() {
    var bsvpk;
    console.log(`using wif ${walletInfo.wif}`)
    //bsvpk = bsv.PrivateKey(walletInfo.wif)
    bsvpk = bsv.PrivateKey.fromWIF(walletInfo.wif)
    return bsvpk
  }

  async function explorer(url) {
    // fetch data from a url endpoint
    const response = await axios.get(url)
    const data = await response.data
    return data
  }

  function getTape() {
      return fs.readFileSync('../app/tape.txt', 'utf-8')
      // return '4c01014c0101934c010388'
  }

  // Here is the main function. It simply gets the tape file contents,
  // puts it into a tx and broadcasts
  const tape = getTape()
  if (tape) {
    console.log(tape)

    const tapeScript = bsv.Script.fromHex(tape)
    // address is mgEWZ3FjNvWCswzLikjGMijsdvdV1kxzo6
    // https://test.whatsonchain.com/address/mgEWZ3FjNvWCswzLikjGMijsdvdV1kxzo6
    // or mgczdj3eMXFH1h8Q8YSW4aMXNLJX2YZWo7
    // get private key
    const pk = getPrivateKey()
    const fromAddress = pk.toAddress()
 
    const url_utxo = `https://api.whatsonchain.com/v1/bsv/test/address/${fromAddress}/unspent`

    ; (async () => {

        // get utxo from our wallet
        const utxos = await explorer(url_utxo)
        let fromUtxo
        // height, tx_pos, tx_hash, value
        // get the first utxo
        for (let i = 0; i<utxos.length; i++) {
            console.log(utxos[i]["tx_hash"])
            console.log(utxos[i]["value"])
            fromUtxo = utxos[i]
            break
        }

        // put utxo in format bsv wants
        fromUtxo.address = fromAddress
        fromUtxo.txId = fromUtxo.tx_hash
        fromUtxo.outputIndex = fromUtxo.tx_pos
        fromUtxo.satoshis = fromUtxo.value
        fromUtxo.script = bsv.Script.buildPublicKeyHashOut(fromAddress).toString()

        // construct a tx
        const tx = new bsv.Transaction()
        const sats = 400
        tx.from(fromUtxo)
        tx.to(fromAddress, sats)
        tx.change(fromAddress)
        //add tape custom script to output
        tx.outputs[0].setScript(tapeScript)
        tx.sign(pk)

    console.log(tx.getFee())

    // broadcast the tx
    axios.post('https://api.whatsonchain.com/v1/bsv/test/tx/raw', {
        txhex: tx.uncheckedSerialize()
    })
    .then(function (response) {
        console.log(`Your tx id is ${response.data}`)
        console.log(`view it at https://test.whatsonchain.com/tx/${response.data}`)
    })
    .catch(function (error) {
        console.log(error)
    })

    })()
  } else {
      console.error(`Tape is empty. Nothing to broadcast.`)
  }

