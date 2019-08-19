\ testing words
\ simple add. 1 + 1 = 2
: testadd recorder-on 1 PUSHDATA 1 PUSHDATA OP_ADD 2 PUSHDATA OP_EQUAL recorder-off cr . ;
: testadd_op recorder-on OP_1 OP_1 OP_ADD OP_2 OP_EQUAL recorder-off cr . ;
\ testadd

\ looping factorial
\ notice I am only recording the necessary calculations for a thread of execution
\ looping outer constructs are not getting recorded for these demos
: fac 1 PUSHDATA swap 1+ 1 ?do i PUSHDATA OP_MUL loop ;
: testfac recorder-on 5 fac 120 PUSHDATA OP_EQUAL recorder-off cr . ;
\ testfac

\ looping square root
\ this is native forth implementation
\ This implementation does not leave a clean stack on the bitcoin script side
\ original forth
\ : sqrt-closer ( square guess -- square guess adjustment) 2dup / over - 2 / ;
\ : sqrt ( square -- root ) 1 begin sqrt-closer dup while + repeat drop nip ;
\ : sqrt-closer ( square guess -- square guess adjustment) OP_2DUP OP_DIV OP_OVER OP_SUB OP_2DIV ;
\ : sqrt ( square -- root ) OP_1 begin sqrt-closer OP_DUP while OP_ADD repeat OP_DROP OP_NIP ;

: xnew  ( n xold   n xnew )
\    2DUP  /  +  2/  ;
    OP_2DUP  OP_DIV  OP_ADD  OP_2DIV  ;

\ This works for 9 and 81 but not 169?
: sqrt  ( n    root )
\        DUP 1 >
\        IF    DUP 2/  ( n  n/2 ) 10 0 DO XNEW LOOP NIP
\        THEN  ;
        OP_DUP OP_2DIV ( n  n/2 ) 10 0 DO xnew LOOP OP_NIP
        ;

: testsqrt recorder-on 9 PUSHDATA sqrt 3 PUSHDATA OP_EQUAL recorder-off cr . ;
\ testsqrt

\ looping greatest common divisor
 : gcd ( a b -- n )
\	OP_2DUP <= if OP_SWAP ( Y X ) then
	begin
		OP_TUCK
		OP_MOD
		dup
	0= until
	OP_DROP ;
: testgcd recorder-on 105 PUSHDATA 28 PUSHDATA gcd 7 PUSHDATA OP_EQUAL recorder-off cr . ;
\ testgcd

\ test high level language Hello compiled to bitcoin script
include hack.fs
: testhack recorder-on hello_compiled OP_5 OP_EQUAL recorder-off cr . ;
\ testhack