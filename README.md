# tape-recorder

What if we make it simple, easy and cheap to do the right thing?

What if you could ...
1) Run your Bitcoin application on your computer ...
2) And a recording of the execution could be saved onto a "tape" ...
3) Then the tape could be stored onto the BSV blockchain, not as data, but as Bitcoin Script?

Tape Recorder does exactly that. It is a Wang B Machine. It records execution as it runs and stores it onto a tape. (A tape is simply a list of computer instructions, also called a thread of execution, that is stored as Bitcoin bytecode in hex format).  

Tape Recorder allows for full attestation of what was executed on your computer. It is Proof of execution of the most minute detail, data and code.  
If it is easy and cheap to do the right thing then people will probably do it. If it is difficult and expensive then they probably wont do the right thing.  

Tape Recorder makes it simple and unobtrusive to store data, important events and significantly, code execution onto the blockchain. Fully attestable.  

Tape Recorder is an individual, personal experience. There is your device, Tape Recorder and the blockchain. (There is no world computer!) Users will have privacy settings for Tape Recorder. Users will be able to choose which events and processing they wish to capture, whether to make the data public or private and when to disclose the data downstream. When the user's role is that of an employee or public official then all of the privacy policies can be determined in an employment contract.

Billions of people with a bitcoin Tape Recorder that is easy and cheap to use means that many threads of execution will be captured. Large blocks will contain lots of Bitcoin Script as well as data. Script execution will be hyper-optimized.
  
Bitcoin scales.  

# When would Tape Recorder be used?
Bitcoin can do everything.  
Tape Recorder can conceviably record the execution of any application that is built upon the Bitcoin opcodes. Since Bitcoin is Turing complete it can be any application.

One example would be an election official performing their duty counting votes. They would start recording. As their computer counts the votes all the instructions are being recorded as bytecode. Invalid ballots are accounted for. Data is grouped and summarized. The recording stops and the code is put on chain as Bitcoin Script. Anyone can validate the vote count by executing the script.

Scientific data that needs to be crunched and attested to is another area.

Any time detail data is filtered, processed, grouped and summarized. Capturing the execution path between the data points is key.

There is obviously a lot of hand waving here. Writing applications that are fully attestable is difficult, but possible.

# Why Bitcoin Script?
Storing data on the blockchain is good. 
Storing executable script is better. Code is a higher form of attestation, more stringent requirements. More effort. More work.

Bitcoin opcodes are single bytes, very compact and efficient to interpret. They are ideally suited for this application.

1) Storing just data means that you are sampling your application state. What happened in between states? If you can capture the execution of your application then you are effectively showing every state of your application. Tape Recorder captures this thread of execution as your application travels between states. It is full attestation.

2) Data could be stored that is logically inconsistent since a miner does not "execute" data. A miner executes code, interprets Bitcoin Script. Supplying code to validate the data means that the data was interpreted by a miner during validation. 

(Invalid script seems to make the Bitcoin burnt and ends the thread of execution. More investigation needs to be done on properly validating the script. One possibility is to do additional validation in Tape Recorder, the higher layers of the application, to prevent burning coin)

# Why Forth?
Tape Recorder is written in Forth.  
Forth is extremely similar to Bitcoin Script. There is very little code to write when translating an application between bitcoin script and Forth. Once the bitcoin opcodes were implemented as Forth words then writing and executing bitcoin script in Forth became very easy and felt natural.  

Here is an example translation. OP_DUP is the bitcoin instruction. dup is the Forth word. 0x76 is the bitcoin bytecode.
```
: OP_DUP dup 0x76 write ;
```
This code tells Forth that when it sees `OP_DUP` it should execute `dup` and write 76 hex to the tape. Simple.

The code to interact with the BSV blockchain was written in JavaScript using the bsv library.

# What if you hate Forth? And hate Bitcoin Script? Hello?
There are compilers that will compile high level C-like language to Bitcoin script. 
One such compiler is BSV Editor. Tape Recorder can execute the script that BSV Editor produces (sort of).
Here is an example of looping in Hello, the language of BSV Editor.
```
cnt = 0x02;
for i in [1 .. 2]
{
	cnt = cnt + i;
}
```
This code compiles to the file hack.hll.script. I had to make some small edits to the script file to make it run. The Tape Recorder runnable code is in hack.fs.

# Layer 1 vs Layer 2
Bitcoin has layers.
Layer 1 Bitcoin is the base protocol as enforced by miners.
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
Transaction Output 0 would be a Metanet OP_RETURN with metadata (attributes) describing the script thread.  
Transaction Output 1 would be the primary thread of execution. i.e. the script. i.e. the tape.  
The thread of execution could continue in the same transaction in outputs 2, 3, etc but what advantage would that have? Maybe to show iterations in the case of looping? Attestation of counter variables? Logical grouping?  
The overriding principle is that the thread of execution should be in a spendable utxo (I think so, until I am proven wrong) so it can be continued by the thread owner.  

Threads of execution can be paused and resumed since the utxo should have the full state of the executing thread.  
When the Layer 2 tape is chained across transactions the tape is "spliced" together, either by Layer 1 digital signatures or by Metanet Layer 2 signatures and pub keys.  

# Notes

All hackathon related files are in the hackathon folder.  

Dave Foderick   
dfoderick@gmail.com

