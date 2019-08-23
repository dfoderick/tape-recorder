\ Newton's method to compute square root using Forth to execute to bitcoin script
\ Note that this algorithm does not give the correct result for some integers (i.e. sqrt(3) <> 2)
\ See https://github.com/dfoderick/tape-recorder
include taperecorder.fs

\ z = (x+1)/2
: guess-start OP_1ADD OP_2DIV ;

\   z = ( x / z + z) / 2
: guess-next ( x y z -- x y newguess)
    OP_DUP ( x y z z)
    OP_3 OP_PICK ( x y z z x)
    OP_SWAP ( x y z x z)
    OP_DIV ( x y z x/z)
    OP_ADD  ( x y x/z+z)
    OP_2DIV ( x y result)
    ;

: sqrt ( n -- y)
    OP_DUP OP_DUP
    guess-start
    \ through each iteration stack looks like [x y z] where z is next guess
    begin
    \ while z < y
    OP_2DUP OP_SWAP OP_LESSTHAN 
    while
        OP_IF
        \   y = z
            OP_NIP OP_DUP
        \   z = ( x / z + z) / 2
            guess-next
        OP_ENDIF
    repeat
    \ close the bitcoin loop by consuming the 0 (false) at TOS
    OP_IF OP_ENDIF
    OP_NIP OP_NIP
;

\ 9 sqrt . 3  ok
\ 6 sqrt . 2  ok
\ 144 sqrt . 12  ok
\ 169 sqrt . 13  ok
\ 180 sqrt . 13  ok
\ 1000000 sqrt . 1000  ok

