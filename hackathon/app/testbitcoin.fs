\ basic bitcoin tests
require ttester.fs
require bitcoin.fs
cr
T{ OP_1 OP_1 OP_ADD -> 2 }T
T{ 1 OP_IFDUP -> 1 1 }T
T{ 0 OP_IFDUP -> 0 }T
T{ 1 5 2 OP_WITHIN -> true }T
T{ 1 5 9 OP_WITHIN -> false }T
T{ 1 OP_NOT -> false }T
T{ 9 OP_NOT -> false }T
T{ 0 OP_NOT -> 1 }T
T{ 0 OP_0NOTEQUAL -> false }T
T{ 1 OP_0NOTEQUAL -> 1 }T
T{ 9 OP_0NOTEQUAL -> 1 }T

T{ OP_1 -> 1 }T
T{ OP_2 -> 2 }T
T{ OP_3 -> 3 }T
T{ OP_4 -> 4 }T
T{ OP_5 -> 5 }T
T{ OP_6 -> 6 }T
T{ OP_7 -> 7 }T
T{ OP_8 -> 8 }T
T{ OP_9 -> 9 }T
T{ OP_10 -> 10 }T
T{ OP_11 -> 11 }T
T{ OP_12 -> 12 }T
T{ OP_13 -> 13 }T
T{ OP_14 -> 14 }T
T{ OP_15 -> 15 }T

\ ttester does not handle strings
T{ s" hello" s" world" OP_CAT s" helloworld" compare -> 0 }T
T{ s" hitape" OP_SIZE rot 2drop -> 6 }T
T{ s" helloworld" 2 bitcoin_left s" he" compare -> 0 }T
T{ s" helloworld" 4 bitcoin_right s" orld" compare -> 0 }T
T{ s" helloworld" 5 OP_SPLIT 2swap 2drop s" hello" compare -> 0 }T
T{ s" helloworld" 5 OP_SPLIT 2drop s" world" compare -> 0 }T
T{ s" helloworld" 1 OP_SPLIT 2swap 2drop s" h" compare -> 0 }T
