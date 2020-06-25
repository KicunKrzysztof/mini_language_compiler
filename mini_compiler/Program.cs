using System;
using System.IO;
using System.Collections.Generic;
using GardensPoint;

public class Compiler
{
    public static List<string> source;
    private static StreamWriter sw;
    public static SyntaxTree tree;
    public static Dictionary<string, Variable> symbolArray = new Dictionary<string, Variable>();
    public static List<SemanticError> semanticErrors = new List<SemanticError>();
    public static List<SyntaxError> syntaxErrors = new List<SyntaxError>();
    public static List<LexError> lexErrors = new List<LexError>();
    private static int varNumGenerator = 0;
    private static int labelNumGenerator = 0;
    public static int Main(string[] args)
    {
        string file;
        FileStream source;
        Console.WriteLine("\nMulti-Pass mini language compiler - Gardens Point");
        file = "input.txt";
        if (args.Length >= 1)
            file = args[0];
        try
        {
            var sr = new StreamReader(file);
            string str = sr.ReadToEnd();
            sr.Close();
            Compiler.source = new List<string>(str.Split(new string[] { "\r\n" }, StringSplitOptions.None));
            source = new FileStream(file, FileMode.Open);
        }
        catch (Exception e)
        {
            Console.WriteLine("\n" + e.Message);
            return 1;
        }
        Scanner scanner = new Scanner(source);
        Parser parser = new Parser(scanner);
        Console.WriteLine();
        sw = new StreamWriter(file + ".il");
        //-----------------------------------------------------lex and syntax analysis:
        if (!parser.Parse() || syntaxErrors.Count != 0 || lexErrors.Count != 0)
        {
            foreach (LexError err in lexErrors)
                Console.WriteLine($"{err._description}, occurs in line: {err._lineNum}");
            foreach (SyntaxError err in syntaxErrors)
                Console.WriteLine($"{err._description}, occurs in line: {err._lineNum}");
            return 1;
        }
        //-----------------------------------------------------semantic analysis:
        tree.SemanticAnalysis();
        if (semanticErrors.Count > 0)
        {
            foreach (SemanticError err in semanticErrors)
                Console.WriteLine($"{err._description}, occurs in line: {err._lineNum}");
            return 1;
        }
        //-----------------------------------------------------code generating:
        GenProlog();
        tree.EmitCode();
        GenEpilog();
        //-----------------------------------------------------
        sw.Close();
        source.Close();
        return 0;
    }

    public static void EmitCode(string instr = null)
    {
        if (instr != "")
            sw.WriteLine(instr);
    }
    public static int GenVarNum() { return varNumGenerator++; }
    public static string GenLabel() { return $"L{labelNumGenerator++}"; }
    private static void GenProlog()
    {
        EmitCode(".assembly extern mscorlib { }");
        EmitCode(".assembly mini_compiler { }");
        EmitCode(".method static void main()");
        EmitCode("{");
        EmitCode(".entrypoint");
        EmitCode(".try");
        EmitCode("{");
        EmitCode(".maxstack  64");
        EmitCode("//------------------------- prolog");
    }
    private static void GenEpilog()
    {
        EmitCode("//------------------------- epilog:");
        EmitCode("leave EndMain");
        EmitCode("}");
        EmitCode("catch [mscorlib]System.Exception");
        EmitCode("{");
        EmitCode("callvirt instance string [mscorlib]System.Exception::get_Message()");
        EmitCode("call void [mscorlib]System.Console::WriteLine(string)");
        EmitCode("leave EndMain");
        EmitCode("}");
        EmitCode("EndMain: ret");
        EmitCode("}");
    }
}
#region variables
public abstract class Variable
{
    public VariableType _type;
    public int _cilNumber, _doNotPutOnStack;
    public string _name;
    public Variable(VariableType type, string name)
    {
        _type = type;
        _name = name;
        _doNotPutOnStack = 0;
    }
    public abstract VariableType GetVarType();
}
public class Bool : Variable
{
    public bool _val;
    public Bool(string name) : base(VariableType.Bool, name) { }
    public override VariableType GetVarType() { return VariableType.Bool; }
}
public class Int : Variable
{
    public int _val;
    public Int(string name) : base(VariableType.Int, name) { }
    public override VariableType GetVarType() { return VariableType.Int; }
}
public class Double : Variable
{
    public double _val;
    public Double(string name) : base(VariableType.Double, name) { }
    public override VariableType GetVarType() { return VariableType.Double; }
}
#endregion
#region errors
public class SemanticError
{
    public int _lineNum;
    public string _description;
    public SemanticError(int lineNum, string description)
    {
        _lineNum = lineNum;
        _description = description;
    }
}
public class SyntaxError
{
    public int _lineNum;
    public string _description;
    public SyntaxError(int lineNum, string description)
    {
        _lineNum = lineNum;
        _description = description;
    }
}
public class LexError
{
    public int _lineNum;
    public string _description;
    public LexError(int lineNum, string description)
    {
        _lineNum = lineNum;
        _description = description;
    }
}
#endregion
#region Enums
public enum VariableType
{
    Int, Double, Bool, NoVariable, SemError
}
public enum PrimaryExpType
{
    Ident, False, True, RealNumber, IntNumber, Exp
}
public enum UnaryExpType
{
    Empty, UnaryExp
}
public enum UnaryOpType
{
    Minus, Not, BitwiseNot, Cast2Double, Cast2Int
}
public enum BitwiseOpType
{
    Empty, Or, And
}
public enum MulOpType
{
    Empty, Mul, Div
}
public enum AddOpType
{
    Empty, Add, Sub
}
public enum RelOpType
{
    Empty, LessThan, GreaterThan, LessEqual, GreaterEqual, Equals, NotEquals
}
public enum LogOpType
{
    Empty, And, Or
}
public enum ExpOpType
{
    Empty, Assign
}
public enum VarType
{
    Int, Double, Bool
}
public enum StatType
{
    Block, Exp, Selection, While, Read, Write, Return
}
public enum MainStatType
{
    Empty, Stat, Decl, DeclStat
}
public enum BlockStatType
{
    Empty, StatList
}
public enum DeclListType
{
    Decl, DeclList
}
public enum StatListType
{
    Stat, StatList
}
public enum SelectionStatType
{
    If, IfElse
}
public enum WriteStatType
{
    Exp, String
}
#endregion
#region SyntaxTree
public abstract class SyntaxTree
{
    public int _line = -1;
    public List<SyntaxTree> _children = new List<SyntaxTree>();
    public List<VariableType> _semChildren = new List<VariableType>();
    public SyntaxTree(int line) { _line = line; }
    public virtual void EmitCode()
    {
        for (int i = 0; i < _children.Count; i++)
            _children[i].EmitCode();
    }
    public virtual VariableType SemanticAnalysis()
    {
        for (int i = 0; i < _children.Count; i++)
            _semChildren.Add(_children[i].SemanticAnalysis());
        return _semChildren.Count > 0 ? _semChildren[0] : VariableType.NoVariable;
    }
    public virtual bool LValue()
    {
        if (_children.Count != 1)
            return false;
        else
            return _children[0].LValue();
    }
    public bool PassSemError()
    {
        for (int i = 0; i < _children.Count; i++)
        {
            if (_semChildren[i] == VariableType.SemError)
                return true;
        }
        return false;
    }
    protected void AddCast(int i)
    {
        SyntaxTree node = new UnaryExp(_line, new UnaryOp(_line, UnaryOpType.Cast2Double), _children[i]);
        node._semChildren = new List<VariableType> { VariableType.NoVariable, VariableType.Int };
        _children[i] = node;
    }
    public virtual string GetIdent() { return _children[0].GetIdent(); }
}
#endregion
#region Expressions
public class Program : SyntaxTree
{
    public Program(int lineNum, SyntaxTree child) : base(lineNum) { _children.Add(child); }
    public override void EmitCode()
    {
        base.EmitCode();
        Compiler.EmitCode("L_END: nop");
    }
}
public class PrimaryExp : SyntaxTree
{
    public PrimaryExpType _type;
    public string _val = "";
    public PrimaryExp(int lineNum, PrimaryExpType type) : base(lineNum) { _type = type; }
    public PrimaryExp(int lineNum, PrimaryExpType type, string val) : this(lineNum, type) { _val = val; }
    public PrimaryExp(int lineNum, PrimaryExpType type, SyntaxTree child) : this(lineNum, type) { _children.Add(child); }
    public override void EmitCode()
    {
        base.EmitCode();
        string code = "";
        switch (_type)
        {
            case PrimaryExpType.Ident:
                if (Compiler.symbolArray[_val]._doNotPutOnStack > 0)
                {
                    --Compiler.symbolArray[_val]._doNotPutOnStack;
                }
                else
                {
                    code = $"ldloc {Compiler.symbolArray[_val]._cilNumber}";
                }
                break;
            case PrimaryExpType.False:
                code = $"ldc.i4.0";
                break;
            case PrimaryExpType.True:
                code = $"ldc.i4.1";
                break;
            case PrimaryExpType.RealNumber:
                code = $"ldc.r8 {_val}";
                break;
            case PrimaryExpType.IntNumber:
                code = $"ldc.i4 {_val}";
                break;
            case PrimaryExpType.Exp:
                code = $"nop";
                break;
        }
        Compiler.EmitCode(code);
    }

    public override VariableType SemanticAnalysis()
    {
        base.SemanticAnalysis();
        if (PassSemError())
            return VariableType.SemError;
        switch (_type)
        {
            case PrimaryExpType.Ident:
                if (!Compiler.symbolArray.ContainsKey(_val))
                {
                    Compiler.semanticErrors.Add(new SemanticError(_line, "Error: undeclared variable"));
                    return VariableType.SemError;
                }
                else
                {
                    return Compiler.symbolArray[_val]._type;
                }
            case PrimaryExpType.False:
                return VariableType.Bool;
            case PrimaryExpType.True:
                return VariableType.Bool;
            case PrimaryExpType.RealNumber:
                return VariableType.Double;
            case PrimaryExpType.IntNumber:
                return VariableType.Int;
            case PrimaryExpType.Exp:
                return _semChildren[0];
            default:
                return VariableType.NoVariable;
        }
    }
    public override bool LValue()
    {
        if (_type == PrimaryExpType.Ident)
            return true;
        else
            return false;
    }
    public override string GetIdent() { return _val; }
}
public class UnaryExp : SyntaxTree
{
    public UnaryExpType _type;
    public UnaryExp(int lineNum, SyntaxTree child) : base(lineNum)
    {
        _children.Add(child);
        _type = UnaryExpType.Empty;
    }
    public UnaryExp(int lineNum, SyntaxTree childOp, SyntaxTree childExp) : base(lineNum)
    {
        _children.Add(childOp);
        _children.Add(childExp);
        _type = UnaryExpType.UnaryExp;
    }
    public override void EmitCode()
    {
        base.EmitCode();
        string code = "";
        if (_type == UnaryExpType.Empty)
            return;
        switch (((UnaryOp)_children[0])._type)
        {
            case UnaryOpType.Minus:
                code = "neg";
                break;
            case UnaryOpType.Not:
                code = "ldc.i4.0\nceq";
                break;
            case UnaryOpType.BitwiseNot:
                code = "not";
                break;
            case UnaryOpType.Cast2Double:
                code = "conv.r8";
                break;
            case UnaryOpType.Cast2Int:
                code = "conv.i4";
                break;
        }
        Compiler.EmitCode(code);
    }

    public override VariableType SemanticAnalysis()
    {
        base.SemanticAnalysis();
        if (PassSemError())
            return VariableType.SemError;
        switch (_type)
        {
            case UnaryExpType.Empty:
                return _semChildren[0];
            case UnaryExpType.UnaryExp:
                switch (((UnaryOp)_children[0])._type)
                {
                    case UnaryOpType.Minus:
                        if (_semChildren[1] == VariableType.Int || _semChildren[1] == VariableType.Double)
                        {
                            return _semChildren[1];
                        }
                        else
                        {
                            Compiler.semanticErrors.Add(new SemanticError(_line, "Error: unary minus operand must has type int or double"));
                            return VariableType.SemError;
                        }
                    case UnaryOpType.Not:
                        if (_semChildren[1] == VariableType.Bool)
                        {
                            return VariableType.Bool;
                        }
                        else
                        {
                            Compiler.semanticErrors.Add(new SemanticError(_line, "Error: logic not operand must has type bool"));
                            return VariableType.SemError;
                        }
                    case UnaryOpType.BitwiseNot:
                        if (_semChildren[1] == VariableType.Int)
                        {
                            return VariableType.Int;
                        }
                        else
                        {
                            Compiler.semanticErrors.Add(new SemanticError(_line, "Error: bitwise not operand must has type int"));
                            return VariableType.SemError;
                        }
                    case UnaryOpType.Cast2Double:
                        return VariableType.Double;
                    case UnaryOpType.Cast2Int:
                        return VariableType.Int;
                    default:
                        return VariableType.NoVariable;
                }
            default:
                return VariableType.NoVariable;
        }
    }
}
public class UnaryOp : SyntaxTree
{
    public UnaryOpType _type;
    public UnaryOp(int lineNum, UnaryOpType type) : base(lineNum) { _type = type; }
    public override void EmitCode() { base.EmitCode(); }
}
public class BitwiseExp : SyntaxTree
{
    public BitwiseOpType _type;
    public BitwiseExp(int lineNum, SyntaxTree childUnExp) : base(lineNum)
    {
        _children.Add(childUnExp);
        _type = BitwiseOpType.Empty;
    }
    public BitwiseExp(int lineNum, SyntaxTree childBitExp, SyntaxTree childUnExp, BitwiseOpType type) : base(lineNum)
    {
        _children.Add(childBitExp);
        _children.Add(childUnExp);
        _type = type;
    }
    public override void EmitCode()
    {
        base.EmitCode();
        switch (_type)
        {
            case BitwiseOpType.Or:
                Compiler.EmitCode("or");
                break;
            case BitwiseOpType.And:
                Compiler.EmitCode("and");
                break;
        }
    }

    public override VariableType SemanticAnalysis()
    {
        base.SemanticAnalysis();
        if (PassSemError())
            return VariableType.SemError;
        if (_type == BitwiseOpType.Empty)
        {
            return _semChildren[0];
        }
        else if (_semChildren[0] == VariableType.Int && _semChildren[1] == VariableType.Int)
        {
            return VariableType.Int;
        }
        else
        {
            Compiler.semanticErrors.Add(new SemanticError(_line, "Error: bitwise and/or operands must have type int"));
            return VariableType.SemError;
        }
    }
}
public class MulExp : SyntaxTree
{
    public MulOpType _type;
    public MulExp(int lineNum, SyntaxTree childBitExp) : base(lineNum)
    {
        _children.Add(childBitExp);
        _type = MulOpType.Empty;
    }
    public MulExp(int lineNum, SyntaxTree childMulExp, SyntaxTree childBitExp, MulOpType type) : base(lineNum)
    {
        _children.Add(childMulExp);
        _children.Add(childBitExp);
        _type = type;
    }
    public override void EmitCode()
    {
        base.EmitCode();
        switch (_type)
        {
            case MulOpType.Mul:
                Compiler.EmitCode("mul");
                break;
            case MulOpType.Div:
                Compiler.EmitCode("div");
                break;
        }
    }

    public override VariableType SemanticAnalysis()
    {
        base.SemanticAnalysis();
        if (PassSemError())
        {
            return VariableType.SemError;
        }
        if (_type == MulOpType.Empty)
        {
            return _semChildren[0];
        }
        else if ((_semChildren[0] == VariableType.Int || _semChildren[0] == VariableType.Double)
            && (_semChildren[1] == VariableType.Int || _semChildren[1] == VariableType.Double))
        {
            if (_semChildren[0] == VariableType.Int && _semChildren[1] == VariableType.Int)
            {
                return VariableType.Int;
            }
            else
            {
                AddCast(_semChildren[0] == VariableType.Int ? 0 : 1);
                return VariableType.Double;
            }
        }
        else
        {
            Compiler.semanticErrors.Add(new SemanticError(_line, "Error: multiplicative operands must have type int or double"));
            return VariableType.SemError;
        }
    }
}
public class AddExp : SyntaxTree
{
    public AddOpType _type;
    public AddExp(int lineNum, SyntaxTree childMulExp) : base(lineNum)
    {
        _children.Add(childMulExp);
        _type = AddOpType.Empty;
    }
    public AddExp(int lineNum, SyntaxTree childAddExp, SyntaxTree childMulExp, AddOpType type) : base(lineNum)
    {
        _children.Add(childAddExp);
        _children.Add(childMulExp);
        _type = type;
    }
    public override void EmitCode()
    {
        base.EmitCode();
        switch (_type)
        {
            case AddOpType.Add:
                Compiler.EmitCode("add");
                break;
            case AddOpType.Sub:
                Compiler.EmitCode("sub");
                break;
        }
    }
    public override VariableType SemanticAnalysis()
    {
        base.SemanticAnalysis();
        if (PassSemError())
        {
            return VariableType.SemError;
        }
        if (_type == AddOpType.Empty)
        {
            return _semChildren[0];
        }
        else if ((_semChildren[0] == VariableType.Int || _semChildren[0] == VariableType.Double)
            && (_semChildren[1] == VariableType.Int || _semChildren[1] == VariableType.Double))
        {
            if (_semChildren[0] == VariableType.Int && _semChildren[1] == VariableType.Int)
            {
                return VariableType.Int;
            }
            else
            {
                AddCast(_semChildren[0] == VariableType.Int ? 0 : 1);
                return VariableType.Double;
            }
        }
        else
        {
            Compiler.semanticErrors.Add(new SemanticError(_line, "Error: additives operands must have type int or double"));
            return VariableType.SemError;
        }
    }
}
public class RelExp : SyntaxTree
{
    public RelOpType _type;
    public RelExp(int lineNum, SyntaxTree childAddExp) : base(lineNum)
    {
        _children.Add(childAddExp);
        _type = RelOpType.Empty;
    }
    public RelExp(int lineNum, SyntaxTree childRelExp, SyntaxTree childAddExp, RelOpType type) : base(lineNum)
    {
        _children.Add(childRelExp);
        _children.Add(childAddExp);
        _type = type;
    }
    public override void EmitCode()
    {
        base.EmitCode();
        string code = "";
        switch (_type)
        {
            case RelOpType.LessThan:
                code = "clt";
                break;
            case RelOpType.GreaterThan:
                code = "cgt";
                break;
            case RelOpType.LessEqual:
                code = "cgt\nldc.i4.0\nceq";
                break;
            case RelOpType.GreaterEqual:
                code = "clt\nldc.i4.0\nceq";
                break;
            case RelOpType.Equals:
                code = "ceq";
                break;
            case RelOpType.NotEquals:
                code = "ceq\nldc.i4.0\nceq";
                break;
        }
        Compiler.EmitCode(code);
    }
    public override VariableType SemanticAnalysis()
    {
        base.SemanticAnalysis();
        if (PassSemError())
        {
            return VariableType.SemError;
        }
        if (_type == RelOpType.Empty)
        {
            return _semChildren[0];
        }
        if ((_semChildren[0] == VariableType.Double || _semChildren[0] == VariableType.Int)
                    && (_semChildren[1] == VariableType.Double || _semChildren[1] == VariableType.Int))
        {
            if (_semChildren[0] != _semChildren[1])
                AddCast(_semChildren[0] == VariableType.Int ? 0 : 1);
            return VariableType.Bool;
        }
        else if ((_type == RelOpType.Equals || _type == RelOpType.NotEquals)
            && _semChildren[0] == VariableType.Bool && _semChildren[1] == VariableType.Bool)
        {
            return VariableType.Bool;
        }
        else
        {
            switch (_type)
            {
                case RelOpType.LessThan:
                    Compiler.semanticErrors.Add(new SemanticError(_line, "Error: LessThan operands must have type int or double"));
                    break;
                case RelOpType.GreaterThan:
                    Compiler.semanticErrors.Add(new SemanticError(_line, "Error: GreaterThan operands must have type int or double"));
                    break;
                case RelOpType.LessEqual:
                    Compiler.semanticErrors.Add(new SemanticError(_line, "Error: LessEqual operands must have type int or double"));
                    break;
                case RelOpType.GreaterEqual:
                    Compiler.semanticErrors.Add(new SemanticError(_line, "Error: GreaterEqual operands must have type int or double"));
                    break;
                case RelOpType.Equals:
                    Compiler.semanticErrors.Add(new SemanticError(_line, "Error: Equals operands must have type int, double or bool"));
                    break;
                case RelOpType.NotEquals:
                    Compiler.semanticErrors.Add(new SemanticError(_line, "Error: NotEquals operands must have type int, double or bool"));
                    break;
            }
        }
        return VariableType.SemError;
    }
}
public class LogExp : SyntaxTree
{
    public LogOpType _type;
    public string _label1 = "", _label2 = "";
    public LogExp(int lineNum, SyntaxTree childRelExp) : base(lineNum)
    {
        _children.Add(childRelExp);
        _type = LogOpType.Empty;
    }
    public LogExp(int lineNum, SyntaxTree childLogExp, SyntaxTree childRelExp, LogOpType type) : base(lineNum)
    {
        _children.Add(childLogExp);
        _children.Add(childRelExp);
        _type = type;
    }
    public override void EmitCode()
    {
        switch (_type)
        {
            case LogOpType.Empty:
                _children[0].EmitCode();
                break;
            case LogOpType.And:
                _children[0].EmitCode();
                _label1 = Compiler.GenLabel();
                _label2 = Compiler.GenLabel();
                if (((LogExp)_children[0])._label1 != "")
                {
                    int leftVal = ((LogExp)_children[0])._type == LogOpType.And ? 0 : 1;
                    Compiler.EmitCode($"{((LogExp)_children[0])._label1}: ldc.i4.{leftVal}");
                    Compiler.EmitCode($"{((LogExp)_children[0])._label2}:");
                }
                Compiler.EmitCode($"brfalse {_label1}");
                _children[1].EmitCode();
                Compiler.EmitCode($"br {_label2}");
                break;
            case LogOpType.Or:
                _children[0].EmitCode();
                _label1 = Compiler.GenLabel();
                _label2 = Compiler.GenLabel();
                if (((LogExp)_children[0])._label1 != "")
                {
                    int leftVal = ((LogExp)_children[0])._type == LogOpType.And ? 0 : 1;
                    Compiler.EmitCode($"{((LogExp)_children[0])._label1}: ldc.i4.{leftVal}");
                    Compiler.EmitCode($"{((LogExp)_children[0])._label2}:");
                }
                Compiler.EmitCode($"brtrue {_label1}");
                _children[1].EmitCode();
                Compiler.EmitCode($"br {_label2}");
                break;
        }
    }
    public override VariableType SemanticAnalysis()
    {
        base.SemanticAnalysis();
        if (PassSemError())
        {
            return VariableType.SemError;
        }
        if (_type == LogOpType.Empty)
        {
            return _semChildren[0];
        }
        else if (_semChildren[0] == VariableType.Bool && _semChildren[1] == VariableType.Bool)
        {
            return VariableType.Bool;
        }
        else
        {
            Compiler.semanticErrors.Add(new SemanticError(_line, "Error: logic and/or operands must have type bool"));
            return VariableType.SemError;
        }
    }
}
public class Exp : SyntaxTree
{
    public ExpOpType _type;
    public Exp(int lineNum, SyntaxTree childLogExp) : base(lineNum)
    {
        _children.Add(childLogExp);
        _type = ExpOpType.Empty;
    }
    public Exp(int lineNum, SyntaxTree childLogExp, SyntaxTree childExp) : base(lineNum)
    {
        _children.Add(childLogExp);
        _children.Add(childExp);
        _type = ExpOpType.Assign;
    }
    public override void EmitCode()
    {
        if (_type == ExpOpType.Assign)
        {
            ++Compiler.symbolArray[GetIdent()]._doNotPutOnStack;
        }
        base.EmitCode();
        switch (_type)
        {
            case ExpOpType.Empty:
                if (((LogExp)_children[0])._label1 != "")
                {
                    int leftVal = ((LogExp)_children[0])._type == LogOpType.And ? 0 : 1;
                    Compiler.EmitCode($"{((LogExp)_children[0])._label1}: ldc.i4.{leftVal}");
                    Compiler.EmitCode($"{((LogExp)_children[0])._label2}:");
                }
                break;
            case ExpOpType.Assign:
                Compiler.EmitCode("dup");
                Compiler.EmitCode($"stloc {Compiler.symbolArray[GetIdent()]._cilNumber}");
                break;
        }
    }
    public override VariableType SemanticAnalysis()
    {
        base.SemanticAnalysis();
        if (PassSemError())
            return VariableType.SemError;
        if (_type == ExpOpType.Empty)
        {
            return _semChildren[0];
        }
        else if (_children[0].LValue())
        {
            if (_semChildren[0] == VariableType.Double)
            {
                if (_semChildren[1] == VariableType.Double)
                {
                    return VariableType.Double;
                }
                else if (_semChildren[1] == VariableType.Int)
                {
                    AddCast(1);
                    return VariableType.Double;
                }
                else
                {
                    Compiler.semanticErrors.Add(new SemanticError(_line, "Error: assign right operand must be type int"));
                    return VariableType.SemError;
                }
            }
            else if (_semChildren[0] == VariableType.Int)
            {
                if (_semChildren[1] == VariableType.Int)
                {
                    return VariableType.Int;
                }
                else
                {
                    Compiler.semanticErrors.Add(new SemanticError(_line, "Error: assign right operand must be type int"));
                    return VariableType.SemError;
                }
            }
            else if (_semChildren[0] == VariableType.Bool)
            {
                if (_semChildren[1] == VariableType.Bool)
                {
                    return VariableType.Bool;
                }
                else
                {
                    Compiler.semanticErrors.Add(new SemanticError(_line, "Error: assign right operand must be type bool"));
                    return VariableType.SemError;
                }
            }
            else
            {
                Compiler.semanticErrors.Add(new SemanticError(_line, "Error: non defined semantic error"));
                return VariableType.SemError;
            }
        }
        else
        {
            Compiler.semanticErrors.Add(new SemanticError(_line, "Error: assign left operand must be identifier"));
            return VariableType.SemError;
        }
    }
}
#endregion
#region Declarations and Statements
public class Decl : SyntaxTree
{
    public string _val;
    public VarType _type;
    public Decl(int lineNum, VarType type, string val) : base(lineNum)
    {
        _val = val;
        _type = type;
    }
    public override void EmitCode()
    {
        base.EmitCode();
        int varNum = Compiler.GenVarNum();
        Compiler.symbolArray[_val]._cilNumber = varNum;
        switch (_type)
        {
            case VarType.Int:
                Compiler.EmitCode($"[{varNum}] int32 _{_val}");
                break;
            case VarType.Double:
                Compiler.EmitCode($"[{varNum}] float64 _{_val}");
                break;
            case VarType.Bool:
                Compiler.EmitCode($"[{varNum}] bool _{_val}");
                break;
        }
    }

    public override VariableType SemanticAnalysis()
    {
        base.SemanticAnalysis();
        if (Compiler.symbolArray.ContainsKey(_val))
        {
            Compiler.semanticErrors.Add(new SemanticError(_line, "Error: variable already declared"));
            return VariableType.SemError;
        }
        else
        {
            switch (_type)
            {
                case VarType.Int:
                    Compiler.symbolArray.Add(_val, new Int(_val));
                    return VariableType.Int;
                case VarType.Double:
                    Compiler.symbolArray.Add(_val, new Double(_val));
                    return VariableType.Double;
                case VarType.Bool:
                    Compiler.symbolArray.Add(_val, new Bool(_val));
                    return VariableType.Bool;
                default:
                    return VariableType.NoVariable;
            }
        }
    }
}
public class Stat : SyntaxTree
{
    public StatType _type;
    public string _ident = "";
    private string _label1, _label2;
    public Stat(int lineNum, StatType type) : base(lineNum) { _type = type; }
    public Stat(int lineNum, StatType type, SyntaxTree _childrentat) : this(lineNum, type) { _children.Add(_childrentat); }
    public Stat(int lineNum, StatType type, SyntaxTree childExp, SyntaxTree _childrentat) : this(lineNum, type)
    {
        _children.Add(childExp);
        _children.Add(_childrentat);
    }
    public Stat(int lineNum, StatType type, string ident) : this(lineNum, type) { _ident = ident; }
    public override void EmitCode()
    {
        switch (_type)
        {
            case StatType.Exp:
                base.EmitCode();
                Compiler.EmitCode("pop");
                break;
            case StatType.While:
                _label1 = Compiler.GenLabel();
                _label2 = Compiler.GenLabel();
                Compiler.EmitCode($"br {_label1}");
                Compiler.EmitCode($"{_label2}:");
                _children[1].EmitCode();
                Compiler.EmitCode($"{_label1}:");
                _children[0].EmitCode();
                Compiler.EmitCode($"brtrue {_label2}");
                break;
            case StatType.Read:
                Compiler.EmitCode("call string [mscorlib]System.Console::ReadLine()");
                switch (Compiler.symbolArray[_ident].GetVarType())
                {
                    case VariableType.Int:
                        Compiler.EmitCode("call int32 [mscorlib]System.Convert::ToInt32(string)");
                        break;
                    case VariableType.Double:
                        Compiler.EmitCode("call float64 [mscorlib]System.Convert::ToDouble(string)");
                        break;
                    case VariableType.Bool:
                        Compiler.EmitCode("call bool [mscorlib]System.Convert::ToBoolean(string)");
                        break;
                }
                Compiler.EmitCode($"stloc {Compiler.symbolArray[_ident]._cilNumber}");
                break;
            case StatType.Return:
                Compiler.EmitCode("br L_END");
                break;
            default:
                base.EmitCode();
                break;
        }
    }

    public override VariableType SemanticAnalysis()
    {
        base.SemanticAnalysis();
        if (PassSemError())
            return VariableType.SemError;
        if (_type == StatType.Read && !Compiler.symbolArray.ContainsKey(_ident))
        {
            Compiler.semanticErrors.Add(new SemanticError(_line, "Error: undeclared variable"));
            return VariableType.SemError;
        }
        else if (_type == StatType.While && _semChildren[0] != VariableType.Bool)
        {
            Compiler.semanticErrors.Add(new SemanticError(_line, "Error: while term must be of type bool"));
            return VariableType.SemError;
        }
        return VariableType.NoVariable;
    }
}
public class BlockStat : SyntaxTree
{
    public BlockStatType _type;
    public BlockStat(int lineNum) : base(lineNum) { _type = BlockStatType.Empty; }
    public BlockStat(int lineNum, SyntaxTree _childrentatList) : base(lineNum)
    {
        _type = BlockStatType.StatList;
        _children.Add(_childrentatList);
    }
    public override void EmitCode() { base.EmitCode(); }
}
public class MainStat : SyntaxTree
{
    public string _val;
    public MainStatType _type;
    public MainStat(int lineNum, MainStatType type) : base(lineNum) { _type = type; }
    public MainStat(int lineNum, MainStatType type, SyntaxTree child) : this(lineNum, type) { _children.Add(child); }
    public MainStat(int lineNum, MainStatType type, SyntaxTree childDecl, SyntaxTree _childrentat) : this(lineNum, type)
    {
        _children.Add(childDecl);
        _children.Add(_childrentat);
    }
    public override void EmitCode()
    {
        switch (_type)
        {
            case MainStatType.Empty:
                Compiler.EmitCode("nop");
                break;
            case MainStatType.Stat:
                base.EmitCode();
                break;
            case MainStatType.Decl:
                Compiler.EmitCode(".locals init (");
                _children[0].EmitCode();
                Compiler.EmitCode(")");
                break;
            case MainStatType.DeclStat:
                Compiler.EmitCode(".locals init (");
                _children[0].EmitCode();
                Compiler.EmitCode(")");
                _children[1].EmitCode();
                break;
        }
    }
}
public class DeclList : SyntaxTree
{
    public DeclListType _type;
    public DeclList(int lineNum, SyntaxTree childDecl) : base(lineNum)
    {
        _type = DeclListType.Decl;
        _children.Add(childDecl);
    }
    public DeclList(int lineNum, SyntaxTree childDeclList, SyntaxTree childDecl) : base(lineNum)
    {
        _type = DeclListType.DeclList;
        _children.Add(childDeclList);
        _children.Add(childDecl);
    }
    public override void EmitCode()
    {
        switch (_type)
        {
            case DeclListType.Decl:
                base.EmitCode();
                break;
            case DeclListType.DeclList:
                _children[0].EmitCode();
                Compiler.EmitCode(",");
                _children[1].EmitCode();
                break;
        }
    }
}
public class StatList : SyntaxTree
{
    public StatListType _type;
    public StatList(int lineNum, SyntaxTree _childrentat) : base(lineNum)
    {
        _type = StatListType.Stat;
        _children.Add(_childrentat);
    }
    public StatList(int lineNum, SyntaxTree _childrentatList, SyntaxTree _childrentat) : base(lineNum)
    {
        _type = StatListType.StatList;
        _children.Add(_childrentatList);
        _children.Add(_childrentat);
    }
    public override void EmitCode() { base.EmitCode(); }
}
public class SelectionStat : SyntaxTree
{
    public SelectionStatType _type;
    private string _label1, _label2;
    public SelectionStat(int lineNum, SyntaxTree childExp, SyntaxTree childStat) : base(lineNum)
    {
        _type = SelectionStatType.If;
        _children.Add(childExp);
        _children.Add(childStat);
    }
    public SelectionStat(int lineNum, SyntaxTree childExp, SyntaxTree childStat1, SyntaxTree childStat2) : base(lineNum)
    {
        _type = SelectionStatType.IfElse;
        _children.Add(childExp);
        _children.Add(childStat1);
        _children.Add(childStat2);
    }
    public override void EmitCode()
    {
        switch (_type)
        {
            case SelectionStatType.If:
                _children[0].EmitCode();
                _label1 = Compiler.GenLabel();
                Compiler.EmitCode($"brfalse {_label1}");
                _children[1].EmitCode();
                Compiler.EmitCode($"{_label1}:");
                break;
            case SelectionStatType.IfElse:
                _children[0].EmitCode();
                _label1 = Compiler.GenLabel();
                Compiler.EmitCode($"brfalse {_label1}");
                _children[1].EmitCode();
                _label2 = Compiler.GenLabel();
                Compiler.EmitCode($"br {_label2}");
                Compiler.EmitCode($"{_label1}:");
                _children[2].EmitCode();
                Compiler.EmitCode($"{_label2}:");
                break;
        }
    }

    public override VariableType SemanticAnalysis()
    {
        base.SemanticAnalysis();
        if (PassSemError())
            return VariableType.SemError;
        if (_semChildren[0] != VariableType.Bool)
            Compiler.semanticErrors.Add(new SemanticError(_line, "Error: if term must be of type bool"));
        return VariableType.SemError;
    }
}
public class WriteStat : SyntaxTree
{
    public string _stringVal = "";
    public WriteStatType _type;
    public WriteStat(int lineNum, WriteStatType type, string stringVal) : base(lineNum)
    {
        _type = type;
        _stringVal = stringVal;
    }
    public WriteStat(int lineNum, WriteStatType type, SyntaxTree childExp) : base(lineNum)
    {
        _type = type;
        _children.Add(childExp);
    }
    public override void EmitCode()
    {
        switch (_type)
        {
            case WriteStatType.Exp:
                switch (_semChildren[0])
                {
                    case VariableType.Int:
                        _children[0].EmitCode();
                        Compiler.EmitCode("call void [mscorlib]System.Console::Write(int32)");
                        break;
                    case VariableType.Double:
                        Compiler.EmitCode("call class [mscorlib]System.Globalization.CultureInfo [mscorlib]System.Globalization.CultureInfo::get_InvariantCulture()");
                        Compiler.EmitCode("ldstr \"{0:0.000000}\"");
                        _children[0].EmitCode();
                        Compiler.EmitCode("box [mscorlib]System.Double");
                        Compiler.EmitCode("call string [mscorlib]System.String::Format(class [mscorlib]System.IFormatProvider, string, object)");
                        Compiler.EmitCode("call void [mscorlib]System.Console::Write(string)");
                        break;
                    case VariableType.Bool:
                        _children[0].EmitCode();
                        Compiler.EmitCode("call void [mscorlib]System.Console::Write(bool)");
                        break;
                }
                break;
            case WriteStatType.String:
                Compiler.EmitCode($"ldstr {_stringVal}");
                Compiler.EmitCode("call void [mscorlib]System.Console::Write(string)");
                break;
        }
    }

    public override VariableType SemanticAnalysis()
    {
        base.SemanticAnalysis();
        if (_type == WriteStatType.Exp && _semChildren[0] != VariableType.Bool && _semChildren[0] != VariableType.Int && _semChildren[0] != VariableType.Double)
        {
            Compiler.semanticErrors.Add(new SemanticError(_line, "Error: this is not a valid expression"));
            return VariableType.SemError;
        }
        return VariableType.NoVariable;
    }
}
#endregion