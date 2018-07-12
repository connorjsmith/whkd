FILES= whkdParser.g4 whkdLexer.g4
OUTPUT_DIR=output/
TEST_DIR=test_files/
ANTLR=java -Xmx500m -cp "/usr/local/lib/antlr-4.7.1-complete.jar:$(CLASSPATH)" org.antlr.v4.Tool
GENERATOR=$(ANTLR) -Dlanguage=CSharp
csharp:
	$(ANTLR) -Dlanguage=CSharp -package Whkd.Parser -o $(OUTPUT_DIR) $(FILES)
test:
	$(ANTLR) -o $(TEST_DIR) $(FILES)
	cd $(TEST_DIR) && javac whkd*.java && cd ..
clean:
	rm -rf $(TEST_DIR)
	rm $(OUTPUT_DIR)*

