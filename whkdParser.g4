parser grammar whkdParser;
options {tokenVocab=whkdLexer;}

profile
    : binding+
    ;

binding
    : (chord ChordSeperator)* chord commandPiece+
    ;

chord
    : ((key | choiceKeyList) ChordElementSeperator)* (key | choiceKeyList)
    ;

key
    : Modifier
    | TerminalKey
    ;
// Matches {Chord1, Chord2, Chord3}
choiceKeyList
    : ChordListOpen (chord ChordListSeperator)* chord ChordListClose
    ;

commandPiece
    : commandSinglePiece
    | commandChoicePiece
    ;
// TODO need to preserve whitespace in command mode
commandSinglePiece
    : CommandText
    ;
// Matches {Command1, Command2, Command3 ... CommandN}
commandList
    : CommandListOpen (commandSinglePiece CommandListSeperator)* commandSinglePiece CommandListClose
    ;
commandChoicePiece
    : (CommandChoiceName? commandList)
    ;
