# DiffEngine.dll v.0.0.1.0 API documentation

Created by 
[mddox](https://github.com/loxsmoke/mddox) on 8/29/2020

# All types

|   |   |   |
|---|---|---|
| [DiffFinder\<T\> Class](#difffindert-class) | [ListExtensions Class](#listextensions-class) | [CharSequence Class](#charsequence-class) |
| [DiffOperation\<T\> Class](#diffoperationt-class) | [StringExtensions Class](#stringextensions-class) | [StringSequence Class](#stringsequence-class) |
| [DiffOperationList\<T\> Class](#diffoperationlistt-class) | [IItemSequence\<T\> Class](#iitemsequencet-class) | [XmlItem Class](#xmlitem-class) |
| [DiffOperationType Enum](#diffoperationtype-enum) | [ItemList\<T1, T2\> Class](#itemlistt1-t2-class) | [XmlItemSequence Class](#xmlitemsequence-class) |
| [MatchStruct Class](#matchstruct-class) | [JsonItem Class](#jsonitem-class) |   |
| [ItemSequenceExtensions Class](#itemsequenceextensions-class) | [JsonItemSequence Class](#jsonitemsequence-class) |   |
# DiffFinder\<T\> Class

Namespace: LoxSmoke.DiffEngine

Generic difference finder. Compares two sequences of elements and finds equalities, insertions and deletions.

## Properties

| Name | Type | Summary |
|---|---|---|
| **Timeout** | TimeSpan? | Diff timeout. Null for no timeout. |
| **TimeoutExpired** | bool | Return True if timeout has expired. False if timeout did not expire or if it was not set. |
| **DiffAbortedByTimeout** | bool | True if diff was stopped due to timeout expiration. <br>False if diff ran to completion. |
## Methods

| Name | Returns | Summary |
|---|---|---|
| **Bisect(T sequence1, T sequence2)** | [DiffOperationList](#diffoperationlistt-class)\<T\> | Find the 'middle snake' of a diff, split the problem in two<br>and return the recursively constructed diff.<br>See Myers 1986 paper: An O(ND) Difference Algorithm and Its Variations. |
| **CreateDiffList(T sequence1, T sequence2)** | [DiffOperationList](#diffoperationlistt-class)\<T\> | Find the differences between two sequences. |
| **HalfMatch(T sequence1, T sequence2)** | MatchStruct |  |
# DiffOperation\<T\> Class

Namespace: LoxSmoke.DiffEngine

Class representing one diff operation (equal, insert or delete) in the list of diff operations.

## Properties

| Name | Type | Summary |
|---|---|---|
| **Operation** | [DiffOperationType](#diffoperationtype-enum) | One of: Insert, Delete or Equal. |
| **OperationText** | string | Operation type as an upper case string. |
| **Contents** | T | The data associated with this diff operation. It is the sequence of items that were either added, unchanged or deleted <br>from the original sequence. When comparing strings this property contains the sequence of characters. |
| **IsInsert** | bool | True if this is an insert operation. |
| **IsDelete** | bool | True if this is a delete operation. |
| **IsEqual** | bool | True if this is an equal operation. |
## Constructors

| Name | Summary |
|---|---|
| **DiffOperation\<T\>([DiffOperationType](#diffoperationtype-enum) operation, T contents)** | Initializes the diff with the provided values. |
## Methods

| Name | Returns | Summary |
|---|---|---|
| **Clone()** | [DiffOperation](#diffoperationt-class)\<T\> | Get the shallow copy of the object. |
| **Equals([Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) obj)** | bool | Determines whether two object instances are equal. |
| **Equals([DiffOperation](#diffoperationt-class)\<T\> otherDiff)** | bool |  |
| **GetHashCode()** | int | Get the hash code. |
| **ToList()** | [DiffOperationList](#diffoperationlistt-class)\<T\> | Create the new list of diff operations containing this object. |
| **ToString()** | string | Return a human-readable string of this Diff. |
# DiffOperationList\<T\> Class

Namespace: LoxSmoke.DiffEngine

The data structure representing a list of Diff objects. 
For example: DELETE:Hello,INSERT:Goodbye,EQUAL:world.
means: delete "Hello", add "Goodbye" and keep " world."

## Properties

| Name | Type | Summary |
|---|---|---|
| **LevenshteinDistance** | int | Compute the Levenshtein distance;  [Wikipedia article here](https://en.wikipedia.org/wiki/Levenshtein_distance) <br>the number of inserted, deleted or substituted items. |
## Constructors

| Name | Summary |
|---|---|
| **DiffOperationList\<T\>()** | Default constructor. |
| **DiffOperationList\<T\>([DiffOperation](#diffoperationt-class)\<T\>[] objs)** | Create the diff list from diff operation objects. |
## Methods

| Name | Returns | Summary |
|---|---|---|
| **Add([DiffOperation](#diffoperationt-class)\<T\> diff)** | void | Add an operation object. |
| **AddFirst([DiffOperation](#diffoperationt-class)\<T\> diff)** | void | Insert the operation object as the first item in the list. |
| **AddRange([DiffOperationList](#diffoperationlistt-class)\<T\> other)** | void |  |
| **CleanupMerge()** | void | Reorder and merge like edit sections.<br>Any edit section can move as long as it doesn't cross an equality. |
| **Clone()** | [DiffOperationList](#diffoperationlistt-class)\<T\> | Clone the list. |
| **Equals([Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) obj)** | bool | Determines whether two object instances are equal. |
| **Equals([DiffOperationList](#diffoperationlistt-class)\<T\> list)** | bool |  |
| **GetHashCode()** | int |  |
| **ToString()** | string | Return the minimal debug string. Show only the number of items in the list. |
## Fields

| Name | Type | Summary |
|---|---|---|
| **Diffs** | [List](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1)\<[DiffOperation](#diffoperationt-class)\<T\>\> | The list of diff operations. |
# DiffOperationType Enum

Namespace: LoxSmoke.DiffEngine

The type of the diff operation

## Values

| Name | Summary |
|---|---|
| **Equal** | No change between two compared sequences |
| **Insert** | New data inserted in the new sequence |
| **Delete** | Data deleted in the original sequence |
# MatchStruct Class

Namespace: LoxSmoke.DiffEngine


## Constructors

| Name | Summary |
|---|---|
| **MatchStruct(T longHead, T longTail, T shortHead, T shortTail, T common)** |  |
## Methods

| Name | Returns | Summary |
|---|---|---|
| **Equals([Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) obj)** | bool | Determines whether two object instances are equal. |
| **Equals(MatchStruct other)** | bool |  |
| **GetHashCode()** | int |  |
| **SwappedShortLong()** | MatchStruct |  |
## Fields

| Name | Type | Summary |
|---|---|---|
| **longHead** | T |  |
| **longTail** | T |  |
| **shortHead** | T |  |
| **shortTail** | T |  |
| **common** | T |  |
# ItemSequenceExtensions Class

Namespace: LoxSmoke.DiffEngine.Extensions

Extension methods for the generic item sequences.

## Methods

| Name | Returns | Summary |
|---|---|---|
| **As(T sequence, [DiffOperationType](#diffoperationtype-enum) operation)** | [DiffOperation](#diffoperationt-class)\<T\> | Create DiffOperation object from the specified item sequence |
| **AsDelete(T sequence)** | [DiffOperation](#diffoperationt-class)\<T\> | Create "Delete" DiffOperation object from the specified item sequence |
| **AsEqual(T sequence)** | [DiffOperation](#diffoperationt-class)\<T\> | Create "Equal" DiffOperation object from the specified item sequence |
| **AsInsert(T sequence)** | [DiffOperation](#diffoperationt-class)\<T\> | Create "Insert" DiffOperation object from the specified item sequence |
| **IsEmpty(T sequence)** | bool | Check if item sequence is empty. |
| **Left(T sequence, int count)** | T | Get the sequence of elements on the left. |
| **LeftExcept(T sequence, int length)** | T | Get the copy of the seqnece with the specified number of items removes. |
| **RightFrom(T sequence, int from)** | T | Get the part of the sequence starting from the specified 0-based index. |
# ListExtensions Class

Namespace: LoxSmoke.DiffEngine.Extensions

Generic list extension methods.

## Methods

| Name | Returns | Summary |
|---|---|---|
| **CommonOverlapLength([List](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1)\<T\> list1, [List](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1)\<T\> list2)** | int | Get the common overlap length of two lists. |
| **CommonPrefix([List](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1)\<T\> list, [List](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1)\<T\> otherList)** | [List](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1)\<T\> | Find and return the common prefix of the two lists. |
| **CommonSuffix([List](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1)\<T\> list, [List](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1)\<T\> otherList)** | [List](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1)\<T\> | Find and return the common suffix of the two lists. |
| **Concat([List](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1)\<T\> list, [List](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1)\<T\> otherList)** | [List](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1)\<T\> | Return the concatenation of the two lists.<br>If one of the lists is empty then method does not create the copy of the list but <br>returns one that is not empty. |
| **EndsWith([List](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1)\<T\> list, [List](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1)\<T\> otherList)** | bool | Check if the list ends with the other list. |
| **FragmentEquals([List](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1)\<T\> list, int from, int len, [List](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1)\<T\> otherList, int otherListStart)** | bool | Check if the part of the list equals to the part of another list. |
| **IndexOf([List](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1)\<T\> list, [List](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1)\<T\> patternList, int startIndex)** | int | Find the specified sequence of the items in the list. |
| **IndexOf([List](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1)\<T\> list, int start, int len, [List](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1)\<T\> patternList, int patternStart, int patternLen)** | int | Find the specified sequence of the items in the list. |
| **Left([List](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1)\<T\> list, int count)** | [List](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1)\<T\> | Get the specified number of the items at the start of the list.<br>Function does not modify the list. |
| **Right([List](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1)\<T\> list, int count)** | [List](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1)\<T\> | Get the specified number of the items at the end of the list.<br>Function does not modify the list. |
| **StartsWith([List](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1)\<T\> list, [List](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1)\<T\> otherList)** | bool | Determines whether the beginning of the list matches a specified list. |
# StringExtensions Class

Namespace: LoxSmoke.DiffEngine.Extensions

String extension methods.

## Methods

| Name | Returns | Summary |
|---|---|---|
| **CommonPrefix(string text1, string text2)** | string | Determine the common prefix of two strings. |
| **CommonSuffix(string text1, string text2)** | string | Determine the common suffix of two strings. |
| **Left(string text, int length)** | string | Get the specified number of the leftmost characters of the string. |
| **Right(string text, int length)** | string | Get the specified number of the rightmost characters of the string. |
# IItemSequence\<T\> Class

Namespace: LoxSmoke.DiffEngine.Interfaces

Sequence of elements that can be compared.

## Properties

| Name | Type | Summary |
|---|---|---|
| **Length** | int | Get length of the sequence. Empty sequence should return 0. |
## Methods

| Name | Returns | Summary |
|---|---|---|
| **CommonOverlapLength(T sequence)** | int | Get the common sequence length. |
| **CommonPrefix(T sequence)** | T | Return the common prefix. |
| **CommonSuffix(T sequence)** | T | Return the common suffix. |
| **Concat(T sequence)** | T | Return the concatenation of this and the other object. |
| **EndsWith(T sequence)** | bool | True if sequence ends with all elements from the specified sequence. |
| **Equals(T other)** | bool | Determines whether two object instances are equal.<br>Must be implemented for diff engine to work. |
| **IndexOf(T sequence, int startFrom)** | int | Find the specified sequence in this sequence.<br>Start search from the specified position. |
| **ItemEquals(int index, T otherSequence, int otherIndex)** | bool | Return true if item at the specified index is equal to the  <br>item at otherIndex position in other sequence. |
| **Mid(int from, int length)** | T | Get the middle of the sequence. |
| **StartsWith(T sequence)** | bool | True if sequence starts with all elements in the specified sequence. |
# ItemList<T1, T2> Class

Namespace: LoxSmoke.DiffEngine.Sequences.Generic

The generic implementation of the item sequence. Implementing class inherits this interface with itself and the item 
type as generic parameters. For example class MySequence : ItemList&lt;MySequence, ItemType&gt; {...}

## Properties

| Name | Type | Summary |
|---|---|---|
| **Data** | [List](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1)\<T2\> | The list of the items. |
| **Length** | int | The number of items in the list. |
## Methods

| Name | Returns | Summary |
|---|---|---|
| **CommonOverlapLength(T1 sequence)** | int |  |
| **CommonPrefix(T1 sequence)** | T1 | Returns the common prefix of two lists. |
| **CommonSuffix(T1 sequence)** | T1 | Returns the common suffix of two lists. |
| **Concat(T1 sequence)** | T1 | Return the concatenation of two sequences. |
| **EndsWith(T1 sequence)** | bool | True if list ends with specified sequence. |
| **Equals(T1 other)** | bool | Determines whether two object instances are equal. |
| **IndexOf(T1 sequence, int startFrom)** | int |  |
| **ItemEquals(int index, T1 otherSequence, int otherIndex)** | bool |  |
| **Mid(int from, int length)** | T1 |  |
| **StartsWith(T1 sequence)** | bool |  |
# JsonItem Class

Namespace: LoxSmoke.DiffEngine.Sequences.Json

JSON item. 

## Properties

| Name | Type | Summary |
|---|---|---|
| **IsStartToken** | bool |  |
| **IsEndToken** | bool |  |
| **IsValueToken** | bool |  |
## Methods

| Name | Returns | Summary |
|---|---|---|
| **Equals([Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) obj)** | bool | Determines whether two object instances are equal. |
| **Equals([JsonItem](#jsonitem-class) token)** | bool | Determines whether two object instances are equal. |
| **GetHashCode()** | int |  |
| **ToPrettyText(bool doIndent)** | string |  |
| **ToString()** | string | Debug-style string. |
## Fields

| Name | Type | Summary |
|---|---|---|
| **TokenType** | JsonToken | Token type. |
| **Depth** | int | Token depth. Useful for diff operations. |
| **Value** | [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) | The token value. |
# JsonItemSequence Class

Namespace: LoxSmoke.DiffEngine.Sequences.Json

Base class: [ItemList](#itemlistt1-t2-class)<[JsonItemSequence](#jsonitemsequence-class), [JsonItem](#jsonitem-class)>

The list of JSON item objects.

## Properties

| Name | Type | Summary |
|---|---|---|
| **Data** | [List](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1)\<[JsonItem](#jsonitem-class)\> | The list of the items. |
| **Length** | int | The number of items in the list. |
## Methods

| Name | Returns | Summary |
|---|---|---|
| **Equals([Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) obj)** | bool | Determines whether two object instances are equal. |
| **Equals([JsonItemSequence](#jsonitemsequence-class) obj)** | bool | Determines whether two object instances are equal. |
| **GetHashCode()** | int |  |
| **Load(string fileName)** | [JsonItemSequence](#jsonitemsequence-class) | Load data from specified JSON input file |
# CharSequence Class

Namespace: LoxSmoke.DiffEngine.Sequences.String

The sequence of characters also known as string.

## Properties

| Name | Type | Summary |
|---|---|---|
| **Length** | int | Get the text length. |
## Constructors

| Name | Summary |
|---|---|
| **CharSequence(string text)** | Create the new CharSequence. |
## Methods

| Name | Returns | Summary |
|---|---|---|
| **CommonOverlapLength([CharSequence](#charsequence-class) sequence)** | int | Determine if the suffix of this string is the prefix of sequence. |
| **CommonPrefix([CharSequence](#charsequence-class) sequence)** | [CharSequence](#charsequence-class) |  |
| **CommonSuffix([CharSequence](#charsequence-class) sequence)** | [CharSequence](#charsequence-class) |  |
| **Concat([CharSequence](#charsequence-class) other)** | [CharSequence](#charsequence-class) |  |
| **EndsWith([CharSequence](#charsequence-class) sequence)** | bool |  |
| **Equals([Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) obj)** | bool | Determines whether two object instances are equal. |
| **Equals([CharSequence](#charsequence-class) sequence)** | bool | Determines whether two object instances are equal. |
| **GetHashCode()** | int |  |
| **IndexOf([CharSequence](#charsequence-class) sequence, int startFrom)** | int |  |
| **ItemEquals(int index, [CharSequence](#charsequence-class) otherSequence, int otherIndex)** | bool |  |
| **Mid(int from, int length)** | [CharSequence](#charsequence-class) |  |
| **StartsWith([CharSequence](#charsequence-class) sequence)** | bool |  |
| **ToString()** | string |  |
## Fields

| Name | Type | Summary |
|---|---|---|
| **Text** | string | The text. |
# StringSequence Class

Namespace: LoxSmoke.DiffEngine.Sequences.TextFile

Base class: [ItemList](#itemlistt1-t2-class)<[StringSequence](#stringsequence-class), int>

The sequence of strings. 
Represents the text file where each element is the line of the file.

## Properties

| Name | Type | Summary |
|---|---|---|
| **Lines** | [IEnumerable](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1)\<string\> | Get all the text lines of this sequence. |
| **Data** | [List](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1)\<int\> | The list of the items. |
| **Length** | int | The number of items in the list. |
## Constructors

| Name | Summary |
|---|---|
| **StringSequence([List](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1)\<int\> lineHashes, [List](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1)\<string\> uniqueLines)** | Create the sequence of strings. |
## Methods

| Name | Returns | Summary |
|---|---|---|
| **Concat([StringSequence](#stringsequence-class) other)** | [StringSequence](#stringsequence-class) | Return the concatenation of two sequences. |
| **Equals([Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) obj)** | bool | Determines whether two object instances are equal. |
| **Equals([StringSequence](#stringsequence-class) other)** | bool | Determines whether two object instances are equal. |
| **GetHashCode()** | int |  |
| **Load(string fileName1, string fileName2)** | ([StringSequence](#stringsequence-class) firstFile, [StringSequence](#stringsequence-class) secondFile) | Load two text files as string sequences for comparison.<br>String sequences for both files share the same unique string list. |
| **Load([IEnumerable](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1)\<string\> lines1, [IEnumerable](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1)\<string\> lines2)** | ([StringSequence](#stringsequence-class) firstFile, [StringSequence](#stringsequence-class) secondFile) | Load two string enumerations as string sequences for comparison.<br>String sequences the same unique string list. |
| **Load([Dictionary](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2)\<string, int\> allLineHashes, [List](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1)\<string\> allLines, [IEnumerable](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1)\<string\> lines)** | [List](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1)\<int\> | Read the list of lines, add unique lines to the dictionary and return the list of hashes. |
## Fields

| Name | Type | Summary |
|---|---|---|
| **UniqueLines** | [List](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1)\<string\> | The list of unique text strings. Index of the string in the list is used as a pseudo-hash code. |
# XmlItem Class

Namespace: LoxSmoke.DiffEngine.Sequences.Xml

XML item.

## Properties

| Name | Type | Summary |
|---|---|---|
| **NodeType** | [XmlNodeType](https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlnodetype) |  |
| **Depth** | int |  |
| **Empty** | bool |  |
| **HasAttributes** | bool |  |
| **Name** | string |  |
| **Value** | string |  |
## Constructors

| Name | Summary |
|---|---|
| **XmlItem()** | Default constructor. |
| **XmlItem([XmlNodeType](https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlnodetype) type, int depth, bool empty, bool hasAttributes, string name, string value)** |  |
## Methods

| Name | Returns | Summary |
|---|---|---|
| **Equals([Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) obj)** | bool | Determines whether two object instances are equal. |
| **Equals([XmlItem](#xmlitem-class) token)** | bool | Determines whether two object instances are equal. |
| **GetHashCode()** | int | Auto-generated hash code function. |
| **ToString()** | string |  |
# XmlItemSequence Class

Namespace: LoxSmoke.DiffEngine.Sequences.Xml

Base class: [ItemList](#itemlistt1-t2-class)<[XmlItemSequence](#xmlitemsequence-class), [XmlItem](#xmlitem-class)>

Sequence of XML items.

## Properties

| Name | Type | Summary |
|---|---|---|
| **Data** | [List](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1)\<[XmlItem](#xmlitem-class)\> | The list of the items. |
| **Length** | int | The number of items in the list. |
## Methods

| Name | Returns | Summary |
|---|---|---|
| **Equals([Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) obj)** | bool | Determines whether two object instances are equal. |
| **Equals([XmlItemSequence](#xmlitemsequence-class) other)** | bool | Determines whether two object instances are equal. |
| **GetHashCode()** | int |  |
| **Load(string fileName, bool trimTextWhitespace)** | [XmlItemSequence](#xmlitemsequence-class) | Load the XML file as a sequence of XML items. |
| **Load([TextReader](https://docs.microsoft.com/en-us/dotnet/api/system.io.textreader) textReader, bool trimTextWhitespace)** | [XmlItemSequence](#xmlitemsequence-class) | Load the XML file as a sequence of XML items. |
| **LoadFromString(string text, bool trimTextWhitespace)** | [XmlItemSequence](#xmlitemsequence-class) | Read the XML string as a sequence of XML items. |
