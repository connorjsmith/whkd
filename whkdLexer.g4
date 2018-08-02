lexer grammar whkdLexer;

KeyChoiceName
    : AlphaNumChar+ ':'
    ;

fragment AlphaNumChar
    : [a-zA-Z0-9]
    ;

fragment ExtendedChar
    : [!@#$%^&*()_+-=]
    ;

ChordListOpen
    : '{'
    ;
ChordListClose
    : '}'
    ;
ChordListSeperator
    : ','
    ;
ChordSeperator
    : ';'
    ;

ChordElementSeperator
    : '+'
    ;

CommandModeBegin
    : '\n\t' -> skip, mode(COMMAND_MODE)
    ;

WS : [ \n\t\r]+ -> skip;

TerminalKey
    : AlphaNumChar
    | '-'
    | '<'
    | '>'
    | '\';\''
    | ':'
    ;

Modifier
    : SHIFT
    | ALT
    | CTRL
    | HYPER
    | META
    | WIN
    ;
META
    : 'Meta'
    | 'meta'
    | 'META'
    ;
HYPER
    : 'Hyper'
    | 'hyper'
    | 'HYPER'
    | 'HYP'
    | 'hyp'
    ;
WIN
    : 'Win'
    | 'WIN'
    | 'Windows'
    | 'WINDOWS'
    ;
SHIFT
    : 'Shift'
    | 'shift'
    | 'SHIFT'
    ;
ALT
    : 'Alt'
    | 'alt'
    | 'ALT'
    ;
CTRL
    : 'Ctrl'
    | 'ctrl'
    | 'CTRL'
    | 'Control'
    | 'control'
    | 'CONTROL'
    ;

mode COMMAND_MODE;

CommandChoiceName
    : AlphaNumChar+ ':'
    ;

CommandIndent
    : '\t' -> skip
    ;

CommandNewLine
    : '\n' -> skip, mode(DEFAULT_MODE);

CommandText
    : (AlphaNumChar | CommandWS | ExtendedChar)+
    | '"'
    | '\''
    ;

CommandListOpen
    : '{'
    ;
CommandListClose
    : '}'
    ;
CommandListSeperator
    : ','
    ;
fragment CommandWS
    : [ \t\r]
    ;
