# PhatAC Cache.bin Parser

## Usage Instructions
* Extract the cache.bin segments using another tool.
* **Chose Output Folder** where the JSON/SQL files will be written to.
* For each segment in Phat AC Cache.bin Parser, **Chose Source.bin** and point it to the approprite decompressed/decrypted segment.bin
* Check **Write JSON Output** to wrote the JSON files. Uncheck this if you're testing/working on the decoder code.
* Click **Do Parse**.

## Known Issues
* Segment 1 parses very roughly.
* Segment 2 parser (Spells) is incomplete and will crash.
* Segment 3 parser is not implemented.
* Segment 5 parser is not implemented.