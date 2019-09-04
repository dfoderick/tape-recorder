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
: ron recorder-on ;
: recorder-off fd close-file throw 0 to fd ;
: roff recorder-off ;
: write to-hex console-or-file ;

\ include bitcoin opcodes here ...
include bitcoin.fs
