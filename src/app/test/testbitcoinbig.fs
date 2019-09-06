\ bitcoin bignum tests
require ttester.fs
include ../bitcoin-big.fs
cr clearstack
T{ OP_1 OP_1 OP_ADD OP_2 OP_EQUAL -> true }T
T{ big 1 OP_IFDUP depth 2 = 2nip -> true }T
T{ big 0 OP_IFDUP depth 1 = nip -> true }T
T{ big 1 big 5 big 2 OP_WITHIN -> true }T
T{ big 1 big 5 big 9 OP_WITHIN -> false }T
T{ big 1 OP_NOT big 0 big= -> true }T
T{ big 9 OP_NOT big 0 big= -> true }T
T{ big 0 OP_NOT big 1 big= -> true }T
T{ big 0 OP_0NOTEQUAL big 0 big= -> true }T
T{ big 1 OP_0NOTEQUAL big 1 big= -> true }T
T{ big 9 OP_0NOTEQUAL big 1 big= -> true }T

T{ OP_1 big 1 big= -> true }T
T{ OP_2 big 2 big= -> true }T
T{ OP_3 big 3 big= -> true }T
T{ OP_4 big 4 big= -> true }T
T{ OP_5 big 5 big= -> true }T
T{ OP_6 big 6 big= -> true }T
T{ OP_7 big 7 big= -> true }T
T{ OP_8 big 8 big= -> true }T
T{ OP_9 big 9 big= -> true }T
T{ OP_10 big 10 big= -> true }T
T{ OP_11 big 11 big= -> true }T
T{ OP_12 big 12 big= -> true }T
T{ OP_13 big 13 big= -> true }T
T{ OP_14 big 14 big= -> true }T
T{ OP_15 big 15 big= -> true }T
