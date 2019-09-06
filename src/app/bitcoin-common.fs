\ common words between regular and big number machines

include lib/altstack.fs
\ defines altstack named alt with only 10 capacity
100 altstack alt

: bitcoin_verify
    true <> if 
        assert( false )
    then
;

: OP_NOP 0x61 write ;

\ =============== Stack words ============================
\ has to consume TOS according to new specification
: OP_RETURN drop 0x6A write ;
: OP_TOALTSTACK alt altpush 0x6B write ;
: OP_FROMALTSTACK alt altpop 0x6C write ;
: OP_DEPTH depth 0x74 write ;
: OP_DROP drop 0x75 write ;
: OP_DUP dup 0x76 write ;
: OP_NIP nip 0x77 write ;
: OP_OVER over 0x78 write ;
: OP_PICK pick 0x79 write ;
: OP_ROLL roll 0x7A write ;
: OP_ROT rot 0x7B write ;
: OP_SWAP swap 0x7C write ;
: OP_TUCK tuck 0x7D write ;
: OP_2DROP 2drop 0x6D write ;
: OP_2DUP 2dup 0x6E write ;
: OP_3DUP dup 2over rot 0x6F write ;
: OP_2OVER 2over 0x70 write ;
: OP_2ROT 2rot 0x71 write ;
: OP_2SWAP 2swap 0x72 write ;

\ ============= Miscellaneous Words =======================
: OP_CODESEPARATOR 0xAB write ;

\ dummy opcodes. These would be used when there is no 1-to-1 correspondence
\ between forth and bitcoin operations.
\ Forth while loop consumes TOS so have to drop on bitcoin side to maintain stack
: OP_WHILE drop ;
