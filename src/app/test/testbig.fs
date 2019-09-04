\ Tests for bignum library
require ../lib/big.4th
require ttester.fs

T{ big 1 big 2 big+ big 3 big= -> true }T
