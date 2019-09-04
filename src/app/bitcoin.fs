\ bitcoin opcodes implemented in forth
\
\ TODO: handle strings as byte array or structure. 
\ string should be represented as one item on stack
\ TODO: allow pushdata to handle multibyte data, for now assumes 1 byte
\ i.e. get size of number on stack and handle endian conversion
\ TODO: crypto functions, if needed, could be c or javascript interop
\ TODO: handle IF/ELSE/ENDIF
\
include functions.fs

include lib/altstack.fs
\ defines altstack named alt with only 10 capacity
10 altstack alt

: bitcoin_verify
    true <> if 
        assert( false )
    then
;

: OP_FALSE false ;
: OP_0 OP_FALSE 0x00 write ;
\ should be 0x01 or true?
\ or do we have to redefine forth true to be 0x01?
: OP_TRUE 1 ;
: OP_1 OP_TRUE 0x51 write ;
: OP_2 2 0x52 write ;
: OP_3 3 0x53 write ;
: OP_4 4 0x54 write ;
: OP_5 5 0x55 write ;
: OP_6 6 0x56 write ;
: OP_7 7 0x57 write ;
: OP_8 8 0x58 write ;
: OP_9 9 0x59 write ;
: OP_10 10 0x5A write ;
: OP_11 11 0x5B write ;
: OP_12 12 0x5C write ;
: OP_13 13 0x5D write ;
: OP_14 14 0x5E write ;
: OP_15 15 0x5F write ;
: OP_16 16 0x60 write ;
: OP_NOP 0x61 write ;

\ push specially encoded 0 through F onto stack
: PUSH1HEX
    dup
    case 
    1 of drop OP_1 endof
    2 of drop OP_2 endof
    3 of drop OP_3 endof
    4 of drop OP_4 endof
    5 of drop OP_5 endof
    6 of drop OP_6 endof
    7 of drop OP_7 endof
    8 of drop OP_8 endof
    9 of drop OP_9 endof
    10 of drop OP_10 endof
    11 of drop OP_11 endof
    12 of drop OP_12 endof
    13 of drop OP_13 endof
    14 of drop OP_14 endof
    15 of drop OP_15 endof
    16 of drop OP_16 endof
    s" PUSH1HEX bad value" exception throw
    endcase
;

\ ( The next byte contains the number of bytes to be pushed onto the stack.)
: OP_PUSHDATA1 0x4C write
    dup SizeOfNumberInBytes write
    \ push data, should enforce size is less than 256
    dup write
; 

\ ( The next two bytes contain the number of bytes to be pushed onto the stack in little endian order)
: OP_PUSHDATA2 0x4D write
    s" not implemented yet" exception throw
;

\ OP_PUSHDATA4 0x4E

\ push minimally encoded data onto stack
: PUSHDATA 
    dup 0 <
    if
        \ TODO: handle negatives
        0x01 write dup write
    else
        dup 16 <= 
        if
            \ 0 - F
            PUSH1HEX
        else
            dup 0x4B <= 
            if
                \ 10 - 4B
                0x01 write dup write
            else
                dup SizeOfNumberInBytes 255 <= 
                if 
                    \ Data size <= 255 bytes
                    OP_PUSHDATA1
                else
                    \ Data size > 255 bytes
                    OP_PUSHDATA2
                then
            then
        then
    then
;

: OP_1NEGATE -1 0x4F write ;
\ TODO: how to do if/then
: OP_IF 0x63 write ;
\ OP_NOTIF 0x64 write ;
\ OP_ELSE 0x67 write ;
: OP_ENDIF 0x68 write ;
\ Marks transaction as invalid if top stack value is not true. The top stack value is removed.
: OP_VERIFY bitcoin_verify 0x69 write ;

\ =============== Stack words ============================
\ has to consume TOS according to new specification
: OP_RETURN drop 0x6A write ;
: OP_TOALTSTACK alt altpush 0x6B write ;
: OP_FROMALTSTACK alt altpop 0x6C write ;
: OP_IFDUP ?dup 0x73 write ; ( If the top stack value is not 0, duplicate it.)
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

\ ================ String Words =================
\ string handling in Forth is different than bitcoin.
\ bitcoin script string is a misnomer. script really just manipulates arrays of bytes
\ TODO: switch to a byte array implementation for working with bitcoin script, but allow forth strings for simplicity
\ for now, think of these as hack ups to experiment with ideas for supporting strings or byte arrays

\ a scratch pad for intermediate string results
create scratch 256 allot

: OP_CAT ( s1 s2 -- stringout)
    2swap ( s2 s1)
    scratch place ( put s1 into scratch)
    scratch +place ( append s2 )
    scratch count ( put s1+s2 back tos)
    0x7E write ;

\ bsv does not have substr, use split
\ : OP_SUBSTR ( addr len begin size -- addr len ) 
\    \ use size as len
\    2 roll ( addr begin size len )
\    drop ( addr begin size )
\    \ add begin to addr
\    swap ( addr size begin )
\    rot ( size begin addr)
\    + ( size newaddr)
\    swap ( newaddr size)

\ Keeps only characters left of the specified point in a string
: bitcoin_left ( addr len newsize -- addr newsize ) 
    nip ( drop the old length and use the new one)
;

\ Keeps only characters right of the specified point in a string.
: bitcoin_right ( addr len size -- addr+size size ) 
    dup >r ( addr len size)
    \ get difference
    - ( addr len-size)
    + ( newaddr )
    \ use new size
    r> ( newaddr size)
;

\ Splits a string, TODO: please refactor
create scratch_split 256 allot
: OP_SPLIT ( addr len num -- right left )
    rot ( len num addr )
    rot ( num addr len )
    2dup 
    2>r ( addr,len to alt stack )
    2 pick ( num addr len num )
    swap ( num addr num len )
    dup >r ( moves len to alt )
    swap - ( num addr len-num )
    r> swap ( num addr len len-num)
    bitcoin_right ( num right )
    scratch_split place ( num )
    2r> ( num addr len )
    rot ( addr len num )
    bitcoin_left ( left )
    scratch_split count ( left right )
    2swap ( right left )
    0x7F write ;

: OP_NUM2BIN
\ TODO
\ Convert the numeric value into a byte sequence of a certain size, taking account of the sign bit. 
\ The byte sequence produced uses the little-endian encoding.
\ 2 4 OP_NUM2BIN -> {0x02, 0x00, 0x00, 0x00}
\ -5 4 OP_NUM2BIN -> {0x05, 0x00, 0x00, 0x80}
    0x80 write ;

: OP_BIN2NUM 
\ TODO
\ Convert the byte sequence into a numeric value, including minimal encoding. 
\ The byte sequence must encode the value in little-endian encoding.
\ {0x02, 0x00, 0x00, 0x00, 0x00} OP_BIN2NUM -> 2. 0x0200000000 in little-endian encoding has value 2.
\ {0x05, 0x00, 0x80} OP_BIN2NUM -> -5 - 0x050080 in little-endian encoding has value -5.
    0x81 write ;

\ ( Pushes the string length of the top element of the stack (without popping it))
\ since Forth string is 2 stack items just need to dup the top of stack
\ this will have to change! Need structure to represent a string as one stack item
: OP_SIZE ( caddr nsize -- caddr nsize nsize) dup 0x82 write ;  

: OP_INVERT invert 0x83 write ;
: OP_AND and 0x84 write ;
: OP_OR or 0x85 write ;
: OP_XOR xor 0x86 write ;
: OP_EQUAL = 0x87 write ;
: OP_EQUALVERIFY = bitcoin_verify 0x88 write ;
: OP_1SUB 1 - 0x8C write ;
: OP_MUL * 0x95 write ;
: OP_DIV / 0x96 write ;
\ 2mul is not implemented yet
\ : OP_2MUL 2 * 0x8D write ;
: OP_2MUL OP_2 OP_MUL ;
\ : OP_2DIV 2/ 0x8E write ;
: OP_2DIV OP_2 OP_DIV ;
: OP_NEGATE negate 0x8F write ;
: OP_ABS abs 0x90 write ;
\ If the input is 0 or 1, it is flipped. Otherwise the output will be 0.
: OP_NOT 
    0= if 
        1 
    else
        0
    then
    0x91 write ;
\ Returns 0 if the input is 0. 1 otherwise.
: OP_0NOTEQUAL
    0= if 
        0
    else
        1
    then
    0x92 write ;
: OP_ADD + 0x93 write ;
: OP_1ADD 1+ 0x8B write ;
: OP_SUB - 0x94 write ;
: OP_MOD mod 0x97 write ;
: OP_LSHIFT lshift 0x98 write ;
: OP_RSHIFT rshift 0x99 write ;
: OP_BOOLAND and 0x9A write ; ( should check that params are bool true or false)
: OP_BOOLOR or 0x9B write ; ( should check that params are bool true or false)
: OP_NUMEQUAL = 0x9C write ;
: OP_NUMEQUALVERIFY = bitcoin_verify 0x9D write ;
: OP_NUMNOTEQUAL <> 0x9E write ;
: OP_LESSTHAN < 0x9F write ;
: OP_GREATERTHAN > 0xA0 write ;
: OP_LESSTHANOREQUAL <= 0xA1 write ;
: OP_GREATERTHANOREQUAL >= 0xA2 write ;
: OP_MIN min 0xA3 write ;
: OP_MAX max 0xA4 write ;
: OP_WITHIN within 0xA5 write ;
\ OP_RIPEMD160 0xA6
\ OP_SHA1 0xA7
\ OP_SHA256 0xA8
\ OP_HASH160 0xA9
\ OP_HASH256 0xAA
: OP_CODESEPARATOR 0xAB write ;
\ OP_CHECKSIG 0xAC
\ OP_CHECKSIGVERIFY 0xAD
\ OP_CHECKMULTISIG 0xAE
\ OP_CHECKMULTISIGVERIFY 0xAF
\ OP_CHECKLOCKTIMEVERIFY 0xB1
\ OP_CHECKSEQUENCEVERIFY 0xB2
\ OP_PUBKEYHASH 0xFD
\ OP_PUBKEY 0xFE
: OP_INVALIDOPCODE 0xFF write ;
: OP_RESERVED 0x50 write ;
\ OP_VER 0x62
\ OP_VERIF 0x65
\ OP_VERIFNOT 0x66
\ OP_RESERVED1 0x89
\ OP_RESERVED2 0x8A
: OP_NOP1 0xB0 write ;

\ dummy opcodes.
\ Forth while loop consumes TOS
: OP_WHILE drop ;
\ 
: bitcoin_drop drop ;