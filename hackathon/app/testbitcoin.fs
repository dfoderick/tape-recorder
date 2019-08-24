\ basic bitcoin tests
include bitcoin.fs

: t-1 assert( 1 OP_1 = ) clearstack ;
: t-add assert( OP_1 OP_1 OP_ADD 2 = ) clearstack ;
: t-ifdup-p assert( 1 OP_IFDUP depth 2 = ) clearstack ;
: t-ifdup-f assert( 0 OP_IFDUP depth 1 = ) clearstack ;
: t-within-p assert( 1 5 3 OP_WITHIN true = ) clearstack ;
: t-within-f assert( 2 5 9 OP_WITHIN false = ) clearstack ;
: t-not-f assert( 1 OP_NOT false = ) clearstack ;
: t-not-p assert( 0 OP_NOT 1 = ) clearstack ;
: t-not-f2 assert( 9 OP_NOT false = ) clearstack ;
: t-0notequal-f assert( 0 OP_0NOTEQUAL false = ) clearstack ;
: t-0notequal-p assert( 1 OP_0NOTEQUAL 1 = ) clearstack ;
: t-0notequal-p2 assert( 9 OP_0NOTEQUAL 1 = ) clearstack ;

cr
t-1
t-add
t-ifdup-p
t-ifdup-f
t-within-p
t-within-f
t-not-f
t-not-p
t-not-f2
t-0notequal-f
t-0notequal-p
t-0notequal-p2

cr \ OP_CAT
: t-cat s" hello" s" world" OP_CAT s" helloworld" compare 0 = . ;
t-cat
clearstack
cr \ op_size
: t-size s" hitape" OP_SIZE 6 = . ;
t-size
clearstack
cr \ op_left
: t-left s" helloworld" 5 OP_LEFT s" hello" compare 0 = . ;
t-left
clearstack
cr \ op_right
: t-right s" helloworld" 5 OP_RIGHT s" world" compare 0 = . ;
t-right
clearstack
cr \ op_substr
: t-substr s" helloworld" 3 3 OP_SUBSTR s" low" compare 0 = . ;
t-substr
clearstack
