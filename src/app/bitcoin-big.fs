\ bitcoin script words with big nuber support
require bitcoin-common.fs
require lib/big.4th

: OP_FALSE s" 0" make_big_number ;
: OP_0 OP_FALSE 0x00 write ;
\ should be 0x01 or true?
\ or do we have to redefine forth true to be 0x01?
: OP_TRUE s" 1" make_big_number ;
: OP_1 OP_TRUE 0x51 write ;
\ TODO: parse string slow. how to optimize?
: OP_2 s" 2" make_big_number 0x52 write ;
: OP_3 s" 3" make_big_number 0x53 write ;
: OP_4 s" 4" make_big_number 0x54 write ;
: OP_5 s" 5" make_big_number 0x55 write ;
: OP_6 s" 6" make_big_number 0x56 write ;
: OP_7 s" 7" make_big_number 0x57 write ;
: OP_8 s" 8" make_big_number 0x58 write ;
: OP_9 s" 9" make_big_number 0x59 write ;
: OP_10 s" 10" make_big_number 0x5A write ;
: OP_11 s" 11" make_big_number 0x5B write ;
: OP_12 s" 12" make_big_number 0x5C write ;
: OP_13 s" 13" make_big_number 0x5D write ;
: OP_14 s" 14" make_big_number 0x5E write ;
: OP_15 s" 15" make_big_number 0x5F write ;
: OP_16 s" 16" make_big_number 0x60 write ;

: OP_IFDUP ( If the top stack value is not 0, duplicate it.)
    dup big0<> if
        \ TODO: something wrong with big>here 
        dup big>here here
    then 0x73 write ; 

: OP_EQUAL big= 0x87 write ;
: OP_EQUALVERIFY big= bitcoin_verify 0x88 write ;

\ If the input is 0 or 1, it is flipped. Otherwise the output will be 0.
: OP_NOT 
    big0= if 
        s" 1"         
    else
        s" 0"
    then
    make_big_number
    0x91 write ;
\ Returns 0 if the input is 0. 1 otherwise.
: OP_0NOTEQUAL
    big0= if 
        s" 0" 
    else
        s" 1"
    then
    make_big_number
    0x92 write ;

: OP_WITHIN ( min max val -- true/false) 
    dup ( min max val val)
    3 roll ( max val val min)
    swap ( max val min val )
    big< ( max val bool)
    invert if
        2drop false
    else
        big< invert
    then
    0xA5 write ;

: OP_ADD big+ 0x93 write ;
: OP_1ADD s" 1" make_big_number big+ 0x8B write ;


