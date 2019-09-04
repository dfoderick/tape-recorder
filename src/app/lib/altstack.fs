\ An implementation of an alternative stack for Forth
\ Forth's return stack could not be used as Bitcoin's alt stack
\ This stack is based on the LIFO stack pattern
: altstack ( n -- )
    create here cell+ , cells allot does> ;

: altpush ( n altstack -- )
    swap over @ ! cell swap +! ;

: altpop ( altstack -- x )
    cell negate over +! dup @ swap over >=
    abort" [altstack underflow] " @ ;

: altclear ( altstack -- )   dup cell+ swap ! ;
: altbounds ( altstack -- addr1 addr2 ) dup @ swap cell+ ; 