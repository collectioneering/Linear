# Pending work

### methods
* only support on lyn with EnableLinearLambda:
  - compile<types...>(string) - compile lambda
  - decode(range/array, Func<int a, int r, byte v, byte> lambda)
    - C# lambda string for weird xors, etc
* find() - find first occurrence of buffer
* decrypt(range/array, algorithm, mode, key, iv?)
* toarray(range) - get byte array
* fromhex(string) - get decoded buffer
* toascii(string) toutf8(string) toutf16(string) - get encoded buffer

### structure deserializer properties
* Source - custom source array (resets absolute offset/length by default)
* SourceRange - custom absolute offset/length
