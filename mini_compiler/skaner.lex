%using QUT.Gppg;
%namespace GardensPoint

IntNumber		([1-9][0-9]*)|0
RealNumber		(0\.[0-9]+)|([1-9][0-9]*\.[0-9]+)
Ident			[A-Za-z][A-Za-z0-9]*
Comments		\/\/(.*)$
Whitespaces		[ \t\r\n]+
String			\"(\\.|[^"\n\\])*\"
Error			[_`@#%':\$\?\\\]\[]+

%%
"program"			{ return (int)Tokens.Program; }
"if"				{ return (int)Tokens.If; }
"else"				{ return (int)Tokens.Else; }
"while"				{ return (int)Tokens.While; }
"read"				{ return (int)Tokens.Read; }
"write"				{ return (int)Tokens.Write; }
"return"			{ return (int)Tokens.Return; }
"int"				{ return (int)Tokens.Int; }
"double"			{ return (int)Tokens.Double; }
"bool"				{ return (int)Tokens.Bool; }
"false"				{ return (int)Tokens.False; }
"true"				{ return (int)Tokens.True; }
"="					{ return (int)Tokens.Assign; }
"||"				{ return (int)Tokens.Or; }
"&&"				{ return (int)Tokens.And; }
"|"					{ return (int)Tokens.BitwiseOr; }
"&"					{ return (int)Tokens.BitwiseAnd; }
"=="				{ return (int)Tokens.Equals; }
"!="				{ return (int)Tokens.NotEquals; }
">"					{ return (int)Tokens.GreaterThan; }
">="				{ return (int)Tokens.GreaterEqual; }
"<"					{ return (int)Tokens.LessThan; }
"<="				{ return (int)Tokens.LessEqual; }
"+"					{ return (int)Tokens.Plus; }
"-"					{ return (int)Tokens.Minus; }
"*"					{ return (int)Tokens.Mul; }
"/"					{ return (int)Tokens.Div; }
"!"					{ return (int)Tokens.Not; }
"~"					{ return (int)Tokens.BitwiseNot; }
"("					{ return (int)Tokens.OpenPar; }
")"					{ return (int)Tokens.ClosePar; }
"{"					{ return (int)Tokens.OpenCurly; }
"}"					{ return (int)Tokens.CloseCurly; }
";"					{ return (int)Tokens.Semicolon; }
{Ident}				{ yylval.val=yytext; return(int)Tokens.Ident; }
{IntNumber}			{ yylval.val=yytext; return(int)Tokens.IntNumber; }
{RealNumber}		{ yylval.val=yytext; return(int)Tokens.RealNumber; }
{String}			{ yylval.val=yytext; return(int)Tokens.String; }
{Comments}			;
{Whitespaces}		;
{Error}				{ Compiler.lexErrors.Add(new LexError(tokLin, String.Format("Lexical error, invalid characters: {0}", yytext)));}

%{
  yylloc = new LexLocation(tokLin, tokCol, tokELin, tokECol);
%}

%%
