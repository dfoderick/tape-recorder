# tape-recorder

What if we make it simple, easy and cheap to do the right thing? Tape Recorder makes it simple and easy to use Bitcoin to record everyday activities.

What if you could ...
1) Run your Bitcoin application on your computer or device ...
2) And a recording of the execution could be saved onto a "tape" ...
3) Then the tape could be stored onto the BSV blockchain, not as data, but as Bitcoin Script?

Tape Recorder does exactly that. It is a Wang B Machine. It records execution as it runs and stores it onto a tape. (A tape is simply a list of computer instructions, also called a thread of execution, that is stored as Bitcoin bytecode in hex format).  

Tape Recorder allows for full attestation of what was executed on your computer. It is Proof of execution of the most minute detail, data and code.  
If it is easy and cheap to do the right thing then people will probably do it. If it is difficult and expensive then they probably wont use Bitcoin.  

Tape Recorder makes it simple and unobtrusive to store data, important events and significantly, code execution onto the blockchain. Fully attestable.  

Tape Recorder is an individual, personal experience. There is your device, Tape Recorder and the blockchain. (There is no world computer!) Users will have privacy settings for Tape Recorder. Users will be able to choose which events and processing they wish to capture, whether to make the data public or private and when to disclose the data downstream. When the user's role is that of an employee or public official then all of the privacy policies can be determined in an employment contract.

Billions of people with a Bitcoin Tape Recorder that is easy and cheap to use means that many threads of execution will be captured. Large blocks will contain lots of Bitcoin Script as well as data. Script execution will be hyper-optimized.
  
Bitcoin scales because it is simple and efficient.  

# When would Tape Recorder be used?
Bitcoin can do everything.  
Tape Recorder can conceviably record the execution of any application that is built upon the Bitcoin opcode primitives. Since Bitcoin is Turing complete you can create any application.

In general, major use cases for Tape Recorder would be centered around performance contracts. Whenever you are subject to specific performance, bound by a perforance contract to perform specific duties, you can attest to your performance by recording the activity. It could be specific performance required under an employment contract or any private contract.

Not all contracts are explicit. And users may wish to caputre other computations and activities even when not bound by a contract.

One example would be an election official performing their duty when counting votes. They would start recording. As their computer counts the votes all the instructions are being recorded as bytecode. Invalid ballots are accounted for. Data is grouped and summarized. The recording stops and the code is put on chain as Bitcoin Script. Anyone can validate the vote count by executing the script.

Scientific data that needs to be crunched and attested to is another area.

Tape Recorder could be used any time detail data is filtered, processed, grouped or summarized. Capturing the execution path between the data points is key.

There is obviously a lot of hand waving here. Writing applications that are fully attestable is difficult, but Tape Recorder shows that it is possible.

# Tape Recorder Attestations
Tape Recorder code has this basic structure. Lets call them attestations.
```
recorder-on <Your code goes here. It can be anything you can imagine!> recorder-off
```
Instructions that are necessary for attestation between the on/off get recorded.  

For example, the following attestation computes factorial of 5. It loops inside the `fac` function.
```
recorder-on 5 fac 120 PUSHDATA OP_EQUAL recorder-off
```
During execution the loop is unrolled. It looks like this (in ASM format).
```
01 01 OP_MUL 02 OP_MUL 03 OP_MUL 04 OP_MUL 05 OP_MUL 78 OP_EQUAL
```
The tape looks like this (Hex format) when it goes on chain.
```
0101010195010295010395010495010595017887
```

# Why Bitcoin Script?
Storing data on the blockchain is good. 
Storing data within executable script is better. Code is a higher form of attestation as it requires more effort to interpret code than to simply store data. 

Using a single data stream that includes data within script allows for data validation rules to be passed in script. Bitcoin opcodes are single bytes, very compact and efficient to interpret. They are ideally suited for this purpose.

1) Storing just data means that you are sampling your application state. What happened in between states? If you can capture the execution of your application then you are effectively showing every state of your application. Tape Recorder captures this thread of execution as your application travels between states. It is full attestation.

2) Data could be stored that is logically inconsistent since a miner does not "execute" data. A miner executes code, interprets Bitcoin Script. Supplying code to validate the data means that the data was interpreted by a miner during validation. 

(Invalid script seems to make the Bitcoin burnt and ends the thread of execution. More investigation needs to be done on properly validating the script. One possibility is to do additional validation in Tape Recorder, the higher layers of the application, to prevent the possibility of coin burn.)

# Why Forth?
Tape Recorder is written in Forth.  
Forth is extremely similar to Bitcoin Script. There is very little code to write when translating an application between bitcoin script and Forth. Once the bitcoin opcodes were implemented as Forth words then writing and executing bitcoin script in Forth became very easy and felt natural.  

Here is an example translation. OP_DUP is the bitcoin instruction. dup is the Forth word. 0x76 is the bitcoin bytecode.
```
: OP_DUP dup 0x76 write ;
```
This code tells Forth that when it sees `OP_DUP` it should execute `dup` and write 76 hex to the tape. Simple.

Once all the necessary Bitcoin opcode primitives were implemented in Forth then it becomes easy to write high level code that executes to bitcoin bytecode.

Here is the high level Forth code that loops while calculating Factorial.

```
: fac ( top of stack is the number of iterations to run )
   1 PUSHDATA 
   swap 1+ 1 
   ?do 
      i PUSHDATA OP_MUL 
   loop ;
```
When Tape Recorder executes the high level Forth code it produces unrolled Bitcoin script bytecode. Here is the output of running `5 fac`.
```
01 01 OP_MUL 02 OP_MUL 03 OP_MUL 04 OP_MUL 05 OP_MUL
```

# Broadcasting attestations

The code to interact with the BSV blockchain was written in JavaScript using the bsv library. It takes the output tape of Tape Recorder and stores it onchain in a Metanet transaction.

# What if you hate Forth? And hate Bitcoin Script? How about Hello?
[BSV Editor](http://www.bowmain.co.uk/BSV/index.html) will compile a high level C-like language called Hello to Bitcoin script. 
Tape Recorder can execute the script that BSV Editor produces.
Here is an example of looping in Hello.
```
cnt = 0x02;
for i in [1 .. 2]
{
	cnt = cnt + i;
}
```
This code compiles to the file hack.hll.script. I had to make some small edits to the script file to make it run. The Tape Recorder executable code is in hack.fs.

There are many other possibilities for high level languages that could be executed by Tape Recorder.  

# A Tape Recorder payment channel

Another way is to run Tape Recorder as a service. Users would submit batch jobs to be run by Tape Recorder using the Bitcoin protocol. Of course the payment channel version of Tape Recorder could be run as a direct IP2IP channel using the techniques in [Ubiquity](https://github.com/dfoderick/ubiquity-hackathon).

# Layer 1 vs Layer 2
Bitcoin has layers.  
Layer 1 Bitcoin is the base protocol as enforced by miner nodes.  
Layer 2 is your Bitcoin application running off chain on your computer.  

The Bitcoin protocol is simply data within script. Because the bitcoin protocol is valuable, it is to be used off chain in your Layer 2 application. Consequently, users execute script in Layer 2 of Bitcoin on their own CPU. Tape Recorder captures these threads of execution while interpreting your bitcoin application in Layer 2 and puts them on chain.

Give a miner Layer 1 script to lock and unlock a utxo.  
Give a miner Layer 2 script for attestation of a thread of execution that you have already ran on your computer.

# Tape Recorder is a Wang B machine
Bitcoin is simple.  
A Wang B machine is a simplified Turing machine.  
It has a Jump instruction so it can loop. It does not have an erase instruction.  

A Wang B machine has a two-way tape. One side is for reading. The other side is for writing.  
The read side is for executing (interpreting) a program. As it reads instructions it writes them to the write side of the tape. The write side of the tape is append only, non-erasable.  

Tape Recorder interprets and executes your Layer 2 application as the read side of the tape. Your application can loop. As it executes your app, Tape Recorder records the instructions onto the write side of the append-only tape. Thus, the loop is unrolled. Loop unrolling happens during runtime of the application, not at compile time.  
Consequently, your application can loop but the miner only sees an unrolled loop. Thus, a miner need not loop to validate the execution of your loopy code.  

Tape recorder is a component (or adapter) that executes along side of your Layer 2 Bitcoin application.

# Metanet and Tape Recorder
In Layer 1, bitcoin transactions are chained together using digital signatures with scriptSig and scriptPubKey.   
Metanet is an overlay structure for Layer 2 applications.   

Tape Recorder is a Layer 2 application that fundamentally stores threads of execution as Bitcoin Script. It should leverage the Metanet protocol as much as possible since there is a lot of good tooling for Metanet but the primary motive is to preserve the continuity of the thread of execution.  

Some ideas ...   
Transaction Output 0 would be a Metanet OP_RETURN with metadata (attributes) describing the script thread (Who, When and Why the thread was executed).  
Transaction Output 1 would be the primary thread of execution. i.e. the script. i.e. the tape.  
The thread of execution could continue in the same transaction in outputs 2, 3, etc but what advantage would that have? Maybe to show iterations in the case of looping? Attestation of counter variables? Logical grouping?  
The overriding principle is that the thread of execution should be in a spendable utxo (I think so, until I am proven wrong) so it can be continued by the thread owner.  

Threads of execution can be paused and resumed since the utxo should have the full state of the executing thread.  
When the Layer 2 tape is chained across transactions the tape is "spliced" together, either by Layer 1 digital signatures or by Metanet Layer 2 signatures and pub keys.  

# Notes

All hackathon related files are in the hackathon folder.  

# Sample code
Samples can be found in testrecorder.fs  

testadd - adds two numbers  
testfac - computes factorial  
testsqrt - computes square root  
testgcd - computes greatest common divisor  
testhack - loops using high level language Hello from BSV Editor

# Installing Tape Recorder
1) Clone the repo  
```
git clone https://github.com/dfoderick/tape-recorder.git
```
2) Install gforth
```
sudo apt install gforth
```
3) Install node and npm
```
sudo apt-get install nodejs
sudo apt install npm
```
4) Install js packages
```
cd ~/tape-recorder
npm install
```
5) Tape Recorder broadcaster will error if tape.txt does not exist so create empty tape
```
touch ~/tape-recorder/hackathon/app/tape.txt
```

# Running Tape Recorder
1) run Tape Recorder broadcaster
```
cd ~/tape-recorder/hackathon/js
node broadcaster auto
```
Your console should say `Waiting for changes ...`

2)  run Tape Recorder
```
cd ~/tape-recorder/hackathon/app
gforth
```
gforth console will be open. Load Tape Recorder and tests.
```
include taperecorder.fs
include testrecorder.fs
```

# Tape Recorder Walkthrough
Run any of the test commands `testadd`, `testfac`, `test`, `testsqrt`, `testgcd`, `testhack`  
Tape Recorder console should respond with `-1 ok` indicating success since -1 in Forth means `true`.

As you run the tests the broadcaster console will show you the tx ids that are getting stored on the blockchain. There may be a slight delay because the node file watcher polls for changes every 5 seconds or so.  

You can view the transactions and the script in any BSV testnet explorer.

Experiment!

The general structure of a Tape Recorder program looks like this:
```
ron <commands> roff .
```
Commands can be concatenated.
```
ron 4 fac op_1 op_add sqrt roff .
```
4! is 24. Then add 1. Then calculate the square root. The result is 5.

# Storing custom script on BSV Testnet

Post txhex to https://api.whatsonchain.com/v1/bsv/test/tx/raw

# TODO
The following are works in progress
* broadcaster.js should write the transaction as a Metanet transaction
* implement all the Bitcoin opcodes. Many are sill commented out in bitcoin.fs
* the interaction between Tape Recorder (Forth) and broadcaster (js) could be improved. Can broadcaster be implemented in Forth?
* Implement a payment channel version of Tape Recorder
* Investigate Webassembly version of bitcoin script interpreter
* Investigate interpreter for mobile devices and IOT
* More apps and use cases

Dave Foderick   
dfoderick@gmail.com

