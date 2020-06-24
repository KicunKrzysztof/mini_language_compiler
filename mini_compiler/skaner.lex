%using QUT.Gppg;
%namespace GardensPoint

IntNumber		([1-9][0-9]*)|0
RealNumber		(0\.[0-9]+)|([1-9][0-9]*\.[0-9]+)
Ident			[A-Za-z][A-Za-z0-9]*
Comments		\/\/(.*)$
Whitespaces		[ \t\r\n]+
String			\"(\\.|[^"\n\\])*\"

%%
"program"			{ yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Tokens.Program; }
"if"				{ yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Tokens.If; }
"else"				{ yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Tokens.Else; }
"while"				{ yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Tokens.While; }
"read"				{ yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Tokens.Read; }
"write"				{ yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Tokens.Write; }
"return"			{ yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Tokens.Return; }
"int"				{ yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Tokens.Int; }
"double"			{ yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Tokens.Double; }
"bool"				{ yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Tokens.Bool; }
"false"				{ yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Tokens.False; }
"true"				{ yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Tokens.True; }
"="					{ yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Tokens.Assign; }
"||"				{ yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Tokens.Or; }
"&&"				{ yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Tokens.And; }
"|"					{ yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Tokens.BitwiseOr; }
"&"					{ yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Tokens.BitwiseAnd; }
"=="				{ yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Tokens.Equals; }
"!="				{ yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Tokens.NotEquals; }
">"					{ yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Tokens.GreaterThan; }
">="				{ yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Tokens.GreaterEqual; }
"<"					{ yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Tokens.LessThan; }
"<="				{ yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Tokens.LessEqual; }
"+"					{ yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Tokens.Plus; }
"-"					{ yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Tokens.Minus; }
"*"					{ yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Tokens.Mul; }
"/"					{ yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Tokens.Div; }
"!"					{ yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Tokens.Not; }
"~"					{ yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Tokens.BitwiseNot; }
"("					{ yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Tokens.OpenPar; }
")"					{ yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Tokens.ClosePar; }
"{"					{ yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Tokens.OpenCurly; }
"}"					{ yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Tokens.CloseCurly; }
";"					{ yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Tokens.Semicolon; }
{Ident}				{ yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); yylval.val=yytext; return(int)Tokens.Ident; }
{IntNumber}			{ yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); yylval.val=yytext; return(int)Tokens.IntNumber; }
{RealNumber}		{ yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); yylval.val=yytext; return(int)Tokens.RealNumber; }
{String}			{ yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); yylval.val=yytext; return(int)Tokens.String; }
{Comments}			;
{Whitespaces}		;


%%
