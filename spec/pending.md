# Pending work

### methods
* only support on lyn with EnableLinearLambda:
  - compile<types...>(string) - compile lambda
  - decode(range/array, Func<int abs, int rel, byte in, byte out> lambda)
    - C# lambda string for weird xors, etc
* find() - find first occurrence of buffer
* decrypt(range/array, algorithm, mode, key, iv?)
* toarray(range) - get byte array
* fromhex(string) - get decoded buffer
* toascii(string) toutf8(string) toutf16(string) - get encoded buffer

### structure deserializer properties
* Source - custom source array (resets absolute offset/length by default)
* SourceRange - custom absolute offset/length

### other
* optional extended format for literal values: value(typename) / typename(value)
* combine literal values / deserialized to be same kind of member
   - just have special kind of expression for deserializers that can be inlined
     (adds some convenience)
    - format like "typename@(expr)"
    - optionally omit type name if it is the top-level expression - "@(expr)"
  - then literals / deserialized can share explicit type-names
    - would need coercion of types to defined member type
    - "var" to just store as-is (internally using System.Object class anyway)
  - motivation: combining just makes more sense, and creating custom real-code
    porting based off of a StructureDefinition (if all relevant APIs were opened
    up) would be easier if deserializers just were expressions

### idk I forgot
* array indexers / length property
