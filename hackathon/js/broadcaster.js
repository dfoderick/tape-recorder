
// this app will read a tape (tape.txt) and put it on chain

const axios = require('axios');
const bsv = require('bsv')
const bsvMnemonic = require('bsv/mnemonic')
const fs = require('fs')

const tapefile = '../app/tape.txt'

// if you get an error Cannot find module './wallet.json'
// it is because wallet.json file is missing!
// You can get one from https://tools.fullcyclemining.com/
// {
//     "mnemonic": "",
//     "wif": "",
//     "legacyAddress": ""
// }
const walletInfo = require(`./wallet.json`)

function getPrivateKey() {
    var bsvpk
    if (walletInfo.wif) {
        console.log(`using wallet wif`)
        bsvpk = bsv.PrivateKey.fromWIF(walletInfo.wif)
    } else {
        console.log(`using wallet mnemonic`)
        const seed = new bsvMnemonic(walletInfo.mnemonic)
        const hdnode = seed.toHDPrivateKey('','testnet')
        //bsvpk = hdnode.privateKey
        bsvpk = hdnode.deriveChild(`m/44'/0'/0'`).privateKey
    }
    return bsvpk
  }

  async function explorer(url) {
    // fetch data from a url endpoint
    console.log(url)
    const response = await axios.get(url)
    const data = await response.data
    return data
  }

  function getTape() {
      return fs.readFileSync(tapefile, 'utf-8')
  }

function storeTape() {
  // puts it into a tx and broadcasts
  const tape = getTape()
  if (tape) {
    console.log(tape)
    const tapeScript = bsv.Script.fromHex(tape)
    console.log(tapeScript.toString())
    // https://test.whatsonchain.com/address/${fromAddress}
    // get private key from wallet
    const pk = getPrivateKey()
    const fromAddress =  bsv.Address.fromPrivateKey(pk)
 
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

        if (!fromUtxo) {
            console.error(`No utxo for ${fromAddress}`)
            process.exit()
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
}

// Here is the main function. It simply gets the tape file contents,
if (process.argv.length <= 2) {
    storeTape()
} else {
    if (process.argv[2] == "auto") {
        console.log(`Waiting for changes to ${tapefile}`)
        fs.watchFile(tapefile, (curr, prev) => {
            console.log(`${tapefile} file Changed`)
            storeTape()
        })
    } else {
        storeTape()
    }
}
