\ use 'include taperecorder.fs' to load this file into your forth environment
\ reload is a command to reset your environment
: reload clearstack s" taperecorder.fs" included ;

\ helper words
: 3drop drop 2drop ;

\ output formatting words
: ## s>d <# # # #> ;
: to-hex hex ## decimal ;

\ File handling words
0 Value fd
\ writes to file if a file is open
: console-or-file fd dup 0> if write-file throw else 3drop then ;
: recorder-on-file w/o create-file throw to fd ;
: recorder-on s" tape.txt" recorder-on-file ;
: recorder-off fd close-file throw ;
: write to-hex console-or-file ;

\ include bitcoin opcodes here ...
include bitcoin.fs

\ testing words
: testadd recorder-on 1 PUSHDATA 1 PUSHDATA OP_ADD 2 PUSHDATA OP_EQUAL recorder-off . ;
\ test

: fac 1 PUSHDATA swap 1+ 1 ?do i PUSHDATA OP_MUL loop ;
: testloop recorder-on 5 fac 120 PUSHDATA OP_EQUAL recorder-off . ;
testloop
