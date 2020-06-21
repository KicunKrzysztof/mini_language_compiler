﻿using System;
using System.IO;
using System.Collections.Generic;
using GardensPoint;

public class Compiler
{

    public static int errors = 0;

    public static List<string> source;

    public static SyntaxTree tree;

    public static Dictionary<string, Variable> symbolArray = new Dictionary<string, Variable>();

    public static List<SemanticError> SemanticErrors = new List<SemanticError>();

    private static StreamWriter sw;

    // arg[0] określa plik źródłowy
    // pozostałe argumenty są ignorowane
    public static int Main(string[] args)
    {
        string file;
        FileStream source;
        Console.WriteLine("\nMulti-Pass mini language compiler - Gardens Point");
        if (args.Length >= 1)
            file = args[0];
        else
        {
            Console.Write("\nsource file:  ");
            file = Console.ReadLine();
        }
        try
        {
            var sr = new StreamReader(file);
            string str = sr.ReadToEnd();
            sr.Close();
            Compiler.source = new System.Collections.Generic.List<string>(str.Split(new string[] { "\r\n" }, System.StringSplitOptions.None));
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
        GenProlog();
        //-----------------------------------------------------lex and syntax analysis:
        parser.Parse();
        //-----------------------------------------------------semantic analysis:
        tree.SemanticAnalysis();
        //-----------------------------------------------------code generating:
        GenEpilog();
        //-----------------------------------------------------
        sw.Close();
        source.Close();
        if (errors == 0)
            Console.WriteLine("  compilation successful\n");
        else
        {
            Console.WriteLine($"\n  {errors} errors detected\n");
            File.Delete(file + ".il");
        }
        return errors == 0 ? 0 : 2;
    }

    public static void EmitCode(string instr = null)
    {
        sw.WriteLine(instr);
    }

    public static void EmitCode(string instr, params object[] args)
    {
        sw.WriteLine(instr, args);
    }

    private static void GenProlog()
    {
        EmitCode(".assembly extern mscorlib { }");
        EmitCode(".assembly mini_compiler { }");
        EmitCode(".method static void main()");
        EmitCode("{");
        EmitCode(".entrypoint");
        EmitCode(".try");
        EmitCode("{");
        EmitCode();

        EmitCode("-------------------------// prolog");
        EmitCode();
    }

    private static void GenEpilog()
    {
        EmitCode("-------------------------// epilog:");
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
    public Variable(VariableType type)
    {
        _type = type;
    }
}
public class Bool : Variable
{
    public bool _val;
    public Bool() : base(VariableType.Bool) { }
}
public class Int : Variable
{
    public int _val;
    public Int() : base(VariableType.Int) { }
}
public class Double : Variable
{
    public double _val;
    public Double() : base(VariableType.Double) { }
}
#endregion
#region errors
public class SemanticError
{
    int _lineNum;
    string _description;
    public SemanticError(int lineNum, string description)
    {
        _lineNum = lineNum;
        _description = description;
    }
}
#endregion
#region Enums
public enum VariableType
{
    Int, Double, Bool, NoVariable
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
    Ident, String
}
#endregion

#region SyntaxTree
public abstract class SyntaxTree
{
    public int _line = -1;
    public List<SyntaxTree> children = new List<SyntaxTree>();
    public List<VariableType> semChildren = new List<VariableType>();
    public SyntaxTree(int line)
    {
        _line = line;
    }
    public abstract string GenCode();
    public virtual VariableType SemanticAnalysis()
    {
        for(int i = 0; i < children.Count; i++)
        {
                semChildren.Add(children[i].SemanticAnalysis());
        }
        return semChildren.Count > 0 ? semChildren[0] : VariableType.NoVariable;
    }
    public virtual bool LValue()
    {
        if (children.Count != 1)
            return false;
        else
            return children[0].LValue();
    }
}
public class Program : SyntaxTree
{
    public Program(int lineNum, SyntaxTree child) : base(lineNum)
    {
        children.Add(child);
    }
    public override string GenCode()
    {
        throw new NotImplementedException();
    }
}
public class PrimaryExp : SyntaxTree
{
    public PrimaryExpType _type;
    public string _val = "";
    public PrimaryExp(int lineNum, PrimaryExpType type) : base(lineNum)
    {
        _type = type;
    }
    public PrimaryExp(int lineNum, PrimaryExpType type, string val) : this(lineNum, type)
    {
        _val = val;
    }
    public PrimaryExp(int lineNum, PrimaryExpType type, SyntaxTree child) : this(lineNum, type)
    {
        children.Add(child);
    }
    public override string GenCode()
    {
        throw new NotImplementedException();
    }
    public override VariableType SemanticAnalysis()
    {
        base.SemanticAnalysis();
        switch (_type)
        {
            case PrimaryExpType.Ident:
                if (!Compiler.symbolArray.ContainsKey(_val))
                {
                    Compiler.SemanticErrors.Add(new SemanticError(_line, "Error: not declared variable"));
                    return VariableType.NoVariable;
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
                return semChildren[0];
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
}
public class UnaryExp : SyntaxTree
{
    public UnaryExpType _type;
    public UnaryExp(int lineNum, SyntaxTree child) : base(lineNum)
    {
        children.Add(child);
        _type = UnaryExpType.Empty;
    }
    public UnaryExp(int lineNum, SyntaxTree childOp, SyntaxTree childExp) : base(lineNum)
    {
        children.Add(childOp);
        children.Add(childExp);
        _type = UnaryExpType.UnaryExp;
    }
    public override string GenCode()
    {
        throw new NotImplementedException();
    }
    public override VariableType SemanticAnalysis()
    {
        base.SemanticAnalysis();
        switch (_type)
        {
            case UnaryExpType.Empty:
                return semChildren[0];
            case UnaryExpType.UnaryExp:
                switch (((UnaryOp)children[0])._type)
                {
                    case UnaryOpType.Minus:
                        if (semChildren[1] == VariableType.Int || semChildren[1] == VariableType.Double)
                        {
                            return semChildren[1];
                        }
                        else
                        {
                            Compiler.SemanticErrors.Add(new SemanticError(_line, "Error: unary minus operand must has type int or double"));
                            return VariableType.NoVariable;
                        }
                    case UnaryOpType.Not:
                        if (semChildren[1] == VariableType.Bool)
                        {
                            return VariableType.Bool;
                        }
                        else
                        {
                            Compiler.SemanticErrors.Add(new SemanticError(_line, "Error: logic not operand must has type bool"));
                            return VariableType.NoVariable;
                        }
                    case UnaryOpType.BitwiseNot:
                        if (semChildren[1] == VariableType.Int)
                        {
                            return VariableType.Int;
                        }
                        else
                        {
                            Compiler.SemanticErrors.Add(new SemanticError(_line, "Error: bitwise not operand must has type int"));
                            return VariableType.NoVariable;
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
    public UnaryOp(int lineNum, UnaryOpType type) : base(lineNum)
    {
        _type = type;
    }
    public override string GenCode()
    {
        throw new NotImplementedException();
    }
}
public class BitwiseExp : SyntaxTree
{
    public BitwiseOpType _type;
    public BitwiseExp(int lineNum, SyntaxTree childUnExp) : base(lineNum)
    {
        children.Add(childUnExp);
        _type = BitwiseOpType.Empty;
    }
    public BitwiseExp(int lineNum, SyntaxTree childBitExp, SyntaxTree childUnExp, BitwiseOpType type) : base(lineNum)
    {
        children.Add(childBitExp);
        children.Add(childUnExp);
        _type = type;
    }
    public override string GenCode()
    {
        throw new NotImplementedException();
    }
    public override VariableType SemanticAnalysis()
    {
        base.SemanticAnalysis();
        if (_type == BitwiseOpType.Empty)
        {
            return semChildren[0];
        }
        else if (semChildren[0] == VariableType.Int && semChildren[1] == VariableType.Int)
        {
            return VariableType.Int;
        }
        else
        {
            Compiler.SemanticErrors.Add(new SemanticError(_line, "Error: bitwise and/or operands must have type int"));
            return VariableType.NoVariable;
        }
    }
}
public class MulExp : SyntaxTree
{
    public MulOpType _type;
    public MulExp(int lineNum, SyntaxTree childBitExp) : base(lineNum)
    {
        children.Add(childBitExp);
        _type = MulOpType.Empty;
    }
    public MulExp(int lineNum, SyntaxTree childMulExp, SyntaxTree childBitExp, MulOpType type) : base(lineNum)
    {
        children.Add(childMulExp);
        children.Add(childBitExp);
        _type = type;
    }
    public override string GenCode()
    {
        throw new NotImplementedException();
    }
    public override VariableType SemanticAnalysis()
    {
        base.SemanticAnalysis();
        if (_type == MulOpType.Empty)
        {
            return semChildren[0];
        }
        else if ((semChildren[0] == VariableType.Int || semChildren[0] == VariableType.Double)
            && (semChildren[1] == VariableType.Int || semChildren[1] == VariableType.Double))
        {
            if (semChildren[0] == VariableType.Int && semChildren[1] == VariableType.Int)
            {
                return VariableType.Int;
            }
            else
            {
                return VariableType.Double;
            }
        }
        else
        {
            Compiler.SemanticErrors.Add(new SemanticError(_line, "Error: multiplicative operands must have type int or double"));
            return VariableType.NoVariable;
        }
    }
}
public class AddExp : SyntaxTree
{
    public AddOpType _type;
    public AddExp(int lineNum, SyntaxTree childMulExp) : base(lineNum)
    {
        children.Add(childMulExp);
        _type = AddOpType.Empty;
    }
    public AddExp(int lineNum, SyntaxTree childAddExp, SyntaxTree childMulExp, AddOpType type) : base(lineNum)
    {
        children.Add(childAddExp);
        children.Add(childMulExp);
        _type = type;
    }
    public override string GenCode()
    {
        throw new NotImplementedException();
    }
    public override VariableType SemanticAnalysis()
    {
        base.SemanticAnalysis();
        if (_type == AddOpType.Empty)
        {
            return semChildren[0];
        }
        else if ((semChildren[0] == VariableType.Int || semChildren[0] == VariableType.Double)
            && (semChildren[1] == VariableType.Int || semChildren[1] == VariableType.Double))
        {
            if (semChildren[0] == VariableType.Int && semChildren[1] == VariableType.Int)
            {
                return VariableType.Int;
            }
            else
            {
                return VariableType.Double;
            }
        }
        else
        {
            Compiler.SemanticErrors.Add(new SemanticError(_line, "Error: additives operands must have type int or double"));
            return VariableType.NoVariable;
        }
    }
}
public class RelExp : SyntaxTree
{
    public RelOpType _type;
    public RelExp(int lineNum, SyntaxTree childAddExp) : base(lineNum)
    {
        children.Add(childAddExp);
        _type = RelOpType.Empty;
    }
    public RelExp(int lineNum, SyntaxTree childRelExp, SyntaxTree childAddExp, RelOpType type) : base(lineNum)
    {
        children.Add(childRelExp);
        children.Add(childAddExp);
        _type = type;
    }
    public override string GenCode()
    {
        throw new NotImplementedException();
    }
    public override VariableType SemanticAnalysis()
    {
        base.SemanticAnalysis();
        if (_type == RelOpType.Empty)
            return semChildren[0];
        VariableType arg1Type = semChildren[0];
        VariableType arg2Type = semChildren[1];
        switch (_type)
        {
            case RelOpType.LessThan:
                if ((arg1Type == VariableType.Double || arg1Type == VariableType.Int)
                    && (arg2Type == VariableType.Double || arg2Type == VariableType.Int))
                {
                    return VariableType.Bool;
                }
                else
                {
                    Compiler.SemanticErrors.Add(new SemanticError(_line, "Error: LessThan operands must have type int or double"));
                    return VariableType.NoVariable;
                }
            case RelOpType.GreaterThan:
                if ((arg1Type == VariableType.Double || arg1Type == VariableType.Int)
                    && (arg2Type == VariableType.Double || arg2Type == VariableType.Int))
                {
                    return VariableType.Bool;
                }
                else
                {
                    Compiler.SemanticErrors.Add(new SemanticError(_line, "Error: GreaterThan operands must have type int or double"));
                    return VariableType.NoVariable;
                }
            case RelOpType.LessEqual:
                if ((arg1Type == VariableType.Double || arg1Type == VariableType.Int)
                    && (arg2Type == VariableType.Double || arg2Type == VariableType.Int))
                {
                    return VariableType.Bool;
                }
                else
                {
                    Compiler.SemanticErrors.Add(new SemanticError(_line, "Error: LessEqual operands must have type int or double"));
                    return VariableType.NoVariable;
                }
            case RelOpType.GreaterEqual:
                if ((arg1Type == VariableType.Double || arg1Type == VariableType.Int)
                    && (arg2Type == VariableType.Double || arg2Type == VariableType.Int))
                {
                    return VariableType.Bool;
                }
                else
                {
                    Compiler.SemanticErrors.Add(new SemanticError(_line, "Error: GreaterEqual operands must have type int or double"));
                    return VariableType.NoVariable;
                }
            case RelOpType.Equals:
                if ((arg1Type == VariableType.Double || arg1Type == VariableType.Int || arg1Type == VariableType.Bool)
                    && (arg2Type == VariableType.Double || arg2Type == VariableType.Int || arg2Type == VariableType.Bool))
                {
                    return VariableType.Bool;
                }
                else
                {
                    Compiler.SemanticErrors.Add(new SemanticError(_line, "Error: Equals operands must have type int, double or bool"));
                    return VariableType.NoVariable;
                }
            case RelOpType.NotEquals:
                if ((arg1Type == VariableType.Double || arg1Type == VariableType.Int || arg1Type == VariableType.Bool)
                    && (arg2Type == VariableType.Double || arg2Type == VariableType.Int || arg2Type == VariableType.Bool))
                {
                    return VariableType.Bool;
                }
                else
                {
                    Compiler.SemanticErrors.Add(new SemanticError(_line, "Error: NotEquals operands must have type int, double or bool"));
                    return VariableType.NoVariable;
                }
            default:
                return VariableType.NoVariable;
        }
    }
}
public class LogExp : SyntaxTree
{
    public LogOpType _type;
    public LogExp(int lineNum, SyntaxTree childRelExp) : base(lineNum)
    {
        children.Add(childRelExp);
        _type = LogOpType.Empty;
    }
    public LogExp(int lineNum, SyntaxTree childLogExp, SyntaxTree childRelExp, LogOpType type) : base(lineNum)
    {
        children.Add(childLogExp);
        children.Add(childRelExp);
        _type = type;
    }
    public override string GenCode()
    {
        throw new NotImplementedException();
    }
    public override VariableType SemanticAnalysis()
    {
        base.SemanticAnalysis();
        if (_type == LogOpType.Empty)
        {
            return semChildren[0];
        }
        else if (semChildren[0] == VariableType.Bool && semChildren[1] == VariableType.Bool)
        {
            return VariableType.Bool;
        }
        else
        {
            Compiler.SemanticErrors.Add(new SemanticError(_line, "Error: logic and/or operands must have type bool"));
            return VariableType.NoVariable;
        }
    }
}
public class Exp : SyntaxTree
{
    public ExpOpType _type;
    public Exp(int lineNum, SyntaxTree childLogExp) : base(lineNum)
    {
        children.Add(childLogExp);
        _type = ExpOpType.Empty;
    }
    public Exp(int lineNum, SyntaxTree childLogExp, SyntaxTree childExp) : base(lineNum)
    {
        children.Add(childLogExp);
        children.Add(childExp);
        _type = ExpOpType.Assign;
    }
    public override string GenCode()
    {
        throw new NotImplementedException();
    }
    public override VariableType SemanticAnalysis()
    {
        base.SemanticAnalysis();
        if (_type == ExpOpType.Empty)
        {
            return semChildren[0];
        }
        else if (children[0].LValue())
        {
            if (semChildren[0] == VariableType.Double)
            {
                if (semChildren[1] == VariableType.Double || semChildren[1] == VariableType.Int)
                {
                    return VariableType.Double;
                }
                else
                {
                    Compiler.SemanticErrors.Add(new SemanticError(_line, "Error: assign right operand must be type int"));
                    return VariableType.NoVariable;
                }
            }
            else if (semChildren[0] == VariableType.Int)
            {
                if (semChildren[1] == VariableType.Int)
                    return VariableType.Int;
                else
                {
                    Compiler.SemanticErrors.Add(new SemanticError(_line, "Error: assign right operand must be type int"));
                    return VariableType.NoVariable;
                }
            }
            else if (semChildren[0] == VariableType.Bool)
            {
                if (semChildren[1] == VariableType.Bool)
                    return VariableType.Bool;
                else
                {
                    Compiler.SemanticErrors.Add(new SemanticError(_line, "Error: assign right operand must be type bool"));
                    return VariableType.NoVariable;
                }
            }
            else
            {
                Compiler.SemanticErrors.Add(new SemanticError(_line, "Error: non defined semantic error"));
                return VariableType.NoVariable;
            }
        }
        else
        {
            Compiler.SemanticErrors.Add(new SemanticError(_line, "Error: assign left operand must be identifier"));
            return VariableType.NoVariable;
        }
    }
}
public class Decl : SyntaxTree
{
    public string _val;
    public VarType _type;
    public Decl(int lineNum, VarType type, string val) : base(lineNum)
    {
        _val = val;
        _type = type;
    }
    public override string GenCode()
    {
        throw new NotImplementedException();
    }
    public override VariableType SemanticAnalysis()
    {
        base.SemanticAnalysis();
        if (Compiler.symbolArray.ContainsKey(_val))
        {
            Compiler.SemanticErrors.Add(new SemanticError(_line, "Error: variable already declared"));
            return VariableType.NoVariable;
        }
        else
        {
            switch (_type)
            {
                case VarType.Int:
                    Compiler.symbolArray.Add(_val, new Int());
                    return VariableType.Int;
                case VarType.Double:
                    Compiler.symbolArray.Add(_val, new Double());
                    return VariableType.Double;
                case VarType.Bool:
                    Compiler.symbolArray.Add(_val, new Bool());
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
    public Stat(int lineNum, StatType type) : base(lineNum)
    {
        _type = type;
    }
    public Stat(int lineNum, StatType type, SyntaxTree childrentat) : this(lineNum, type)
    {
        children.Add(childrentat);
    }
    public Stat(int lineNum, StatType type, SyntaxTree childExp, SyntaxTree childrentat) : this(lineNum, type)
    {
        children.Add(childExp);
        children.Add(childrentat);
    }
    public Stat(int lineNum, StatType type, string ident) : this(lineNum, type)
    {
        _ident = ident;
    }
    public override string GenCode()
    {
        throw new NotImplementedException();
    }
    public override VariableType SemanticAnalysis()
    {
        base.SemanticAnalysis();
        if (_type == StatType.Read && !Compiler.symbolArray.ContainsKey(_ident))
        {
            Compiler.SemanticErrors.Add(new SemanticError(_line, "Error: identifier not declared"));
        }
        else if (_type == StatType.While && semChildren[0] != VariableType.Bool)
        {
            Compiler.SemanticErrors.Add(new SemanticError(_line, "Error: while term must be of type bool"));
        }
        return VariableType.NoVariable;
    }
}
public class BlockStat : SyntaxTree
{
    public BlockStatType _type;
    public BlockStat(int lineNum) : base(lineNum)
    {
        _type = BlockStatType.Empty;
    }
    public BlockStat(int lineNum, SyntaxTree childrentatList) : base(lineNum)
    {
        _type = BlockStatType.StatList;
        children.Add(childrentatList);
    }

    public override string GenCode()
    {
        throw new NotImplementedException();
    }
}
public class MainStat : SyntaxTree
{
    public string _val;
    public MainStatType _type;
    public MainStat(int lineNum, MainStatType type) : base(lineNum)
    {
        _type = type;
    }
    public MainStat(int lineNum, MainStatType type, SyntaxTree child) : this(lineNum, type)
    {
        children.Add(child);
    }
    public MainStat(int lineNum, MainStatType type, SyntaxTree childDecl, SyntaxTree childrentat) : this(lineNum, type)
    {
        children.Add(childDecl);
        children.Add(childrentat);
    }
    public override string GenCode()
    {
        throw new NotImplementedException();
    }
}
public class DeclList : SyntaxTree
{
    public DeclListType _type;
    public DeclList(int lineNum, SyntaxTree childDecl) : base(lineNum)
    {
        _type = DeclListType.Decl;
        children.Add(childDecl);
    }
    public DeclList(int lineNum, SyntaxTree childDeclList, SyntaxTree childDecl) : base(lineNum)
    {
        _type = DeclListType.DeclList;
        children.Add(childDeclList);
        children.Add(childDecl);
    }
    public override string GenCode()
    {
        throw new NotImplementedException();
    }
}
public class StatList : SyntaxTree
{
    public StatListType _type;
    public StatList(int lineNum, SyntaxTree childrentat) : base(lineNum)
    {
        _type = StatListType.Stat;
        children.Add(childrentat);
    }
    public StatList(int lineNum, SyntaxTree childrentatList, SyntaxTree childrentat) : base(lineNum)
    {
        _type = StatListType.StatList;
        children.Add(childrentatList);
        children.Add(childrentat);
    }
    public override string GenCode()
    {
        throw new NotImplementedException();
    }
}
public class SelectionStat : SyntaxTree
{
    public SelectionStatType _type;
    public SelectionStat(int lineNum, SyntaxTree childExp, SyntaxTree childrentat) : base(lineNum)
    {
        _type = SelectionStatType.If;
        children.Add(childExp);
        children.Add(childrentat);
    }
    public SelectionStat(int lineNum, SyntaxTree childExp, SyntaxTree childrentat1, SyntaxTree childrentat2) : base(lineNum)
    {
        _type = SelectionStatType.IfElse;
        children.Add(childExp);
        children.Add(childrentat1);
        children.Add(childrentat2);
    }
    public override string GenCode()
    {
        throw new NotImplementedException();
    }
    public override VariableType SemanticAnalysis()
    {
        base.SemanticAnalysis();
        if (semChildren[0] != VariableType.Bool)
            Compiler.SemanticErrors.Add(new SemanticError(_line, "Error: if term must be of type bool"));
        return VariableType.NoVariable;
    }
}
public class WriteStat : SyntaxTree
{
    public string _stringVal = "", _ident = "";
    public WriteStatType _type;
    public WriteStat(int lineNum, WriteStatType type, string stringOrIdent) : base(lineNum)
    {
        _type = type;
        switch (_type)
        {
            case WriteStatType.Ident:
                _ident = stringOrIdent;
                break;
            case WriteStatType.String:
                _stringVal = stringOrIdent;
                break;
        }
    }
    public override string GenCode()
    {
        throw new NotImplementedException();
    }
    public override VariableType SemanticAnalysis()
    {
        base.SemanticAnalysis();
        if (_type == WriteStatType.Ident && !Compiler.symbolArray.ContainsKey(_ident))
        {
            Compiler.SemanticErrors.Add(new SemanticError(_line, "Error: identifier not declared"));
        }
        return VariableType.NoVariable;
    }
}
#endregion