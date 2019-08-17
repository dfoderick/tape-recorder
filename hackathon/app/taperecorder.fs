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
\ simple add. 1 + 1 = 2
: testadd recorder-on 1 PUSHDATA 1 PUSHDATA OP_ADD 2 PUSHDATA OP_EQUAL recorder-off cr . ;
\ test

\ looping factorial
\ notice I am only recording the necessary calculations for a thread of execution
\ looping outer constructs are not getting recorded for these demos
: fac 1 PUSHDATA swap 1+ 1 ?do i PUSHDATA OP_MUL loop ;
: testfac recorder-on 5 fac 120 PUSHDATA OP_EQUAL recorder-off cr . ;
\ testfac

\ looping square root
\ this is native forth implementation
\ : sqrt -1 TUCK DO  2 +  DUP +LOOP  2/ ;
\ this is the implementation I ended with because it was easier to convert
: sqrt-closer ( square guess -- square guess adjustment) OP_2DUP OP_DIV OP_OVER OP_SUB 2 PUSHDATA OP_DIV ;
: sqrt ( square -- root ) 1 PUSHDATA begin sqrt-closer OP_DUP while OP_ADD repeat OP_DROP OP_NIP ;
: testsqrt recorder-on 9 PUSHDATA sqrt 3 PUSHDATA OP_EQUAL recorder-off cr . ;
\ testsqrt

\ looping greatest common divisor
\ this algo is difficult to get right in gforth
\ there are discrepancies around how bitcoin consumes TOS in OP_IF
\ : gcd ( a b -- n )
\  begin OP_DUP while OP_TUCK OP_MOD repeat OP_DROP ;
\  begin ?dup while tuck mod repeat ;
 : gcd ( X Y )
\	OP_2DUP <= if OP_SWAP ( Y X ) then
	begin
		OP_TUCK
		OP_MOD
		dup
	0= until
	OP_DROP ;
: testgcd recorder-on 105 PUSHDATA 28 PUSHDATA gcd 7 PUSHDATA OP_EQUAL recorder-off cr . ;
\ testgcd
