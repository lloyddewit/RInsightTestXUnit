/*
 Copyright(C) 2023 Stephen Lloyd
 
 This program is free software: you can redistribute it and/or modify
 it under the terms of the GNU General Public License as published by
 the Free Software Foundation, either version 3 of the License, or
 (at your option) any later version.

 This program is distributed in the hope that it will be useful,
 but WITHOUT ANY WARRANTY; without even the implied warranty of
 MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 GNU General Public License for more details.

 You should have received a copy of the GNU General Public License
 along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/

using RInsight;
using System.Collections.Specialized;
using System.Collections;
using Microsoft.VisualStudio.TestPlatform.CoreUtilities;

namespace RInsightTestXUnit;

public class RInsightTestXUnit
{
    [Fact]
    public void Test1()
    {
        Assert.True(Class1.ReturnTrue());
    }

    [Fact]
    public void TestGetLstLexemes()
    {
        var clsRScript = new RInsight.clsRScript(" ");

        // test lexeme list - identifiers and standard operators
        var lstExpected = new List<string>(new string[] { "a", "::", "b", ":::", "ba", "$", "c", "@", "d", "^", "e", ":", "ea", "%%", "f", "%/%", "g", "%*%", "h", "%o%", "i", "%x%", "j", "%in%", "k", "/", "l", "*", "m", "+", "n", "-", "o", "<", "p", ">", "q", "<=", "r", ">=", "s", "==", "t", "!=", "u", "!", "v", "&", "wx", "&&", "y", "|", "z", "||", "a2", "~", "2b", "->", "c0a", "->>", "d0123456789a", "<-", "1234567890", "<<-", "e0a1b2", "=", "345f6789" });
        var lstActual = clsRScript.GetLstLexemes("a::b:::ba$c@d^e:ea%%f%/%g%*%h%o%i%x%j%in%k/l*m+n-o<p>q<=r>=s==t!=u!v&wx&&y|z||a2~2b->c0a->>d0123456789a<-1234567890<<-e0a1b2=345f6789");
        Assert.Equal(lstExpected, lstActual);

        // test lexeme list - separators, brackets, line feeds, user-defined operators and variable names with '.' and '_'
        lstExpected = new List<string>(new string[] { ",", "ae", ";", "af", "\r", "ag", "\n", "(", "ah", ")", "\r\n", "ai", "{", "aj", "}", "ak", "[", "al", "]", "al", "[[", "am", "]]", "_ao", "%>%", "|>", ".ap", "%aq%", ".ar_2", "%asat%", "au_av.awax" });
        lstActual = clsRScript.GetLstLexemes(",ae;af" + "\r" + "ag" + "\n" + "(ah)" + "\r\n" + "ai{aj}ak[al]al[[am]]_ao%>%|>.ap" + "%aq%.ar_2%asat%au_av.awax");
        Assert.Equal(lstExpected, lstActual);

        // test lexeme list - spaces
        lstExpected = new List<string>(new string[] { " ", "+", "ay", "-", " ", "az", "  ", "::", "ba", "   ", "%*%", "   ", "bb", "   ", "<<-", "    ", "bc", " ", "\r", "  ", "bd", "   ", "\n", "    ", "be", "   ", "\r\n", "  ", "bf", " " });
        lstActual = clsRScript.GetLstLexemes(" +ay- az  ::ba   %*%   bb   <<-    bc " + "\r" + "  bd   " + "\n" + "    be   " + "\r\n" + "  bf ");
        Assert.Equal(lstExpected, lstActual);

        // test lexeme list - string literals
        lstExpected = new List<string>(new string[] { "\"a\"", "+", "\"bf\"", "%%", "\"bga\"", "%/%", "\"bgba\"", "%in%", "\"bgbaa\"", ">=", @"""~!@#$%^&*()_[] {} \|;:',./<>? """, ",", "\" bgbaaa\"", "\r", "\"bh\"", "\n", "\"bi\"", "\r\n", "\"bj\"", "{", "\"bk\"", "[[", "\"bl\"", "%>%", "\"bm\"", "%aq%", "\"bn\"", " ", "+", "\"bn\"", "-", " ", "\"bo\"", "  ", "::", "\"bq\"", "   ", "<<-", "    ", "\"br\"", " ", "\r", "  ", "\"bs\"", "   ", "\n", "    ", "\"bt\"", "   ", "\r\n", "  ", "\"bu\"", " " });
        lstActual = clsRScript.GetLstLexemes("\"a\"+\"bf\"%%\"bga\"%/%\"bgba\"%in%\"bgbaa\">=" + @"""~!@#$%^&*()_[] {} \|;:',./<>? "","" bgbaaa""" + "\r" + "\"bh\"" + "\n" + "\"bi\"" + "\r\n" + "\"bj\"{\"bk\"[[\"bl\"%>%\"bm\"%aq%\"bn\" +\"bn\"- \"bo\"  ::" + "\"bq\"   <<-    \"br\" " + "\r" + "  \"bs\"   " + "\n" + "    \"bt\"   " + "\r\n" + "  \"bu\" ");
        Assert.Equal(lstExpected, lstActual);

        // test lexeme list - comments
        lstExpected = new List<string>(new string[] { "#", "\n", "c", "#", "\n", "ca", "#", "\n", "+", "#", "\n", "%/%", "#", "\n", "%in%", "#", "\n", ">=", @"#~!@#$%^&*()_[]{}\|;:',./<>?#", "\n", " ", "#", "\n", "  ", @"#~!@#$%^&*()_[] {} \|;:',./<>?", "\n", "#cb", "\n", "#cba", "\n", "# \",\" cbaa ", "\n", "#", "\r", "#cc", "\r", "#cca", "\r\n", "# ccaa ", "\r\n" });
        lstActual = clsRScript.GetLstLexemes("#" + "\n" + "c#" + "\n" + "ca#" + "\n" + "+#" + "\n" + "%/%#" + "\n" + "%in%#" + "\n" + @">=#~!@#$%^&*()_[]{}\|;:',./<>?#" + "\n" + " #" + "\n" + @"  #~!@#$%^&*()_[] {} \|;:',./<>?" + "\n" + "#cb" + "\n" + "#cba" + "\n" + "# \",\" cbaa " + "\n" + "#" + "\r" + "#cc" + "\r" + "#cca" + "\r\n" + "# ccaa " + "\r\n");
        Assert.Equal(lstExpected, lstActual);
    }

    [Fact]
    public void TestGetLstTokens()
    {
        var rScript = new clsRScript("");

        // test token list - RSyntacticName
        string strInput = "._+.1+.a+a+ba+baa+a_b+c12+1234567890+2.3+1e6+" + "abcdefghijklmnopqrstuvwxyz+`a`+`a b`+`[[`+`d,ae;af`+`(ah)`+`ai{aj}`+" + @"`~!@#$%^&*()_[] {} \|;:',./<>?`+`%%a_2ab%`+`%ac%`+`[[""b""]]n[[[o][p]]]`+" + "`if`+`else`+`while`+`repeat`+`for`+`in`+`function`+`return`+`else`+`next`+`break`";
        var lstInput = rScript.GetLstLexemes(strInput);
        string strExpected = "._(RSyntacticName), +(ROperatorBinary), .1(RSyntacticName), " + "+(ROperatorBinary), .a(RSyntacticName), +(ROperatorBinary), a(RSyntacticName), " + "+(ROperatorBinary), ba(RSyntacticName), +(ROperatorBinary), baa(RSyntacticName), " + "+(ROperatorBinary), a_b(RSyntacticName), +(ROperatorBinary), c12(RSyntacticName), " + "+(ROperatorBinary), 1234567890(RSyntacticName), +(ROperatorBinary), " + "2.3(RSyntacticName), +(ROperatorBinary), 1e6(RSyntacticName), +(ROperatorBinary), " + "abcdefghijklmnopqrstuvwxyz(RSyntacticName), +(ROperatorBinary), `a`(RSyntacticName), " + "+(ROperatorBinary), `a b`(RSyntacticName), +(ROperatorBinary), `[[`(RSyntacticName), " + "+(ROperatorBinary), `d,ae;af`(RSyntacticName), +(ROperatorBinary), " + "`(ah)`(RSyntacticName), +(ROperatorBinary), `ai{aj}`(RSyntacticName), +(ROperatorBinary), " + @"`~!@#$%^&*()_[] {} \|;:',./<>?`(RSyntacticName), +(ROperatorBinary), " + "`%%a_2ab%`(RSyntacticName), +(ROperatorBinary), `%ac%`(RSyntacticName), " + "+(ROperatorBinary), `[[\"b\"]]n[[[o][p]]]`(RSyntacticName), +(ROperatorBinary), " + "`if`(RSyntacticName), +(ROperatorBinary), `else`(RSyntacticName), +(ROperatorBinary), " + "`while`(RSyntacticName), +(ROperatorBinary), `repeat`(RSyntacticName), " + "+(ROperatorBinary), `for`(RSyntacticName), +(ROperatorBinary), `in`(RSyntacticName), " + "+(ROperatorBinary), `function`(RSyntacticName), +(ROperatorBinary), " + "`return`(RSyntacticName), +(ROperatorBinary), `else`(RSyntacticName), " + "+(ROperatorBinary), `next`(RSyntacticName), +(ROperatorBinary), " + "`break`(RSyntacticName), ";
        string strActual = GetLstTokensAsString(rScript.GetLstTokens(lstInput));
        Assert.Equal(strExpected, strActual);

        // test token list - RBracket, RSeparator
        strInput = "d,ae;af" + "\r" + "ag" + "\n" + "(ah)" + "\r\n" + "ai{aj}";
        lstInput = rScript.GetLstLexemes(strInput);
        strExpected = "d(RSyntacticName), ,(RSeparator), ae(RSyntacticName), ;(REndStatement), " + "af(RSyntacticName), " + "\r" + "(REndStatement), ag(RSyntacticName), " + "\n" + "(REndStatement), ((RBracket), ah(RSyntacticName), )(RBracket), " + "\r\n" + "(REndStatement), ai(RSyntacticName), {(RBracket), aj(RSyntacticName), }(REndScript), ";
        strActual = GetLstTokensAsString(rScript.GetLstTokens(lstInput));
        Assert.Equal(strExpected, strActual);

        // test token list - RSpace
        strInput = " + ay + az + ba   +   bb   +    bc " + "\r" + "  bd   " + "\n" + "    be   " + "\r\n" + "  bf ";
        lstInput = rScript.GetLstLexemes(strInput);
        strExpected = " (RSpace), +(ROperatorUnaryRight),  (RSpace), ay(RSyntacticName), " + " (RSpace), +(ROperatorBinary),  (RSpace), az(RSyntacticName),  (RSpace), " + "+(ROperatorBinary),  (RSpace), ba(RSyntacticName),    (RSpace), " + "+(ROperatorBinary),    (RSpace), bb(RSyntacticName),    (RSpace), " + "+(ROperatorBinary),     (RSpace), bc(RSyntacticName),  (RSpace), " + "\r" + "(REndStatement),   (RSpace), bd(RSyntacticName),    (RSpace), " + "\n" + "(REndStatement),     (RSpace), be(RSyntacticName),    (RSpace), " + "\r\n" + "(REndStatement),   (RSpace), bf(RSyntacticName),  (RSpace), ";
        strActual = GetLstTokensAsString(rScript.GetLstTokens(lstInput));
        Assert.Equal(strExpected, strActual);

        // test token list - RStringLiteral
        strInput = "'a',\"bf\",'bga',\"bgba\",'bgbaa'," + @"""~!@#$%^&*()_[] {} \|;:',./<>? "","" bgbaaa""" + "\r" + "'bh'" + "\n" + "\"bi\"" + "\r\n" + "'bj'{\"bk\",'bl',\"bm\",'bn' ,\"bn\", 'bo'  ," + "\"bq\"   ,    'br' " + "\r" + "  \"bs\"   " + "\n" + "    'bt'   " + "\r\n" + @"  ""bu"" '~!@#$%^&*()_[] {} \|;:"",./<>? '";
        lstInput = rScript.GetLstLexemes(strInput);
        strExpected = "'a'(RStringLiteral), ,(RSeparator), \"bf\"(RStringLiteral), " + ",(RSeparator), 'bga'(RStringLiteral), ,(RSeparator), " + "\"bgba\"(RStringLiteral), ,(RSeparator), 'bgbaa'(RStringLiteral), " + @",(RSeparator), ""~!@#$%^&*()_[] {} \|;:',./<>? ""(RStringLiteral), " + ",(RSeparator), \" bgbaaa\"(RStringLiteral), " + "\r" + "(REndStatement), 'bh'(RStringLiteral), " + "\n" + "(REndStatement), \"bi\"(RStringLiteral), " + "\r\n" + "(REndStatement), 'bj'(RStringLiteral), {(RBracket), \"bk\"(RStringLiteral), " + ",(RSeparator), 'bl'(RStringLiteral), ,(RSeparator), " + "\"bm\"(RStringLiteral), ,(RSeparator), 'bn'(RStringLiteral),  (RSpace), " + ",(RSeparator), \"bn\"(RStringLiteral), ,(RSeparator),  (RSpace), " + "'bo'(RStringLiteral),   (RSpace), ,(RSeparator), " + "\"bq\"(RStringLiteral),    (RSpace), ,(RSeparator),     (RSpace), " + "'br'(RStringLiteral),  (RSpace), " + "\r" + "(REndStatement),   (RSpace), \"bs\"(RStringLiteral),    (RSpace), " + "\n" + "(REndStatement),     (RSpace), 'bt'(RStringLiteral),    (RSpace), " + "\r\n" + "(REndStatement),   (RSpace), \"bu\"(RStringLiteral),  (RSpace), " + @"'~!@#$%^&*()_[] {} \|;:"",./<>? '(RStringLiteral), ";
        strActual = GetLstTokensAsString(rScript.GetLstTokens(lstInput));
        Assert.Equal(strExpected, strActual);

        // test token list - RComment 
        strInput = "#" + "\n" + "c#" + "\n" + "ca#" + "\n" + "d~#" + "\n" + " #" + "\n" + @"  #~!@#$%^&*()_[] {} \|;:',./<>?" + "\n" + "#cb" + "\n" + "#cba" + "\n" + "# \",\" cbaa " + "\n" + "#" + "\r" + "#cc" + "\r" + "#cca" + "\r\n" + "# ccaa " + "\r\n" + "#" + "\n" + "e+f#" + "\n" + " #not ignored comment";
        lstInput = rScript.GetLstLexemes(strInput);
        strExpected = "#(RComment), " + "\n" + "(RNewLine), c(RSyntacticName), #(RComment), " + "\n" + "(REndStatement), ca(RSyntacticName), #(RComment), " + "\n" + "(REndStatement), d(RSyntacticName), ~(ROperatorUnaryLeft), #(RComment), " + "\n" + "(REndStatement),  (RSpace), #(RComment), " + "\n" + @"(RNewLine),   (RSpace), #~!@#$%^&*()_[] {} \|;:',./<>?(RComment), " + "\n" + "(RNewLine), #cb(RComment), " + "\n" + "(RNewLine), #cba(RComment), " + "\n" + "(RNewLine), # \",\" cbaa (RComment), " + "\n" + "(RNewLine), #(RComment), " + "\r" + "(RNewLine), #cc(RComment), " + "\r" + "(RNewLine), #cca(RComment), " + "\r\n" + "(RNewLine), # ccaa (RComment), " + "\r\n" + "(RNewLine), #(RComment), " + "\n" + "(RNewLine), e(RSyntacticName), +(ROperatorBinary), f(RSyntacticName), #(RComment), " + "\n" + "(REndScript),  (RSpace), #not ignored comment(RComment), ";

        strActual = GetLstTokensAsString(rScript.GetLstTokens(lstInput));
        Assert.Equal(strExpected, strActual);

        // test token list - standard operators ROperatorUnaryLeft, ROperatorUnaryRight, ROperatorBinary
        strInput = "a::b:::ba$c@d^e:ea%%f%/%g%*%h%o%i%x%j%in%k/l*m+n-o<p>q<=r>=s==t!=u!v&wx&&y|z" + "||a2~2b->c0a->>d0123456789a<-1234567890<<-e0a1b2=345f6789+a/(b)*((c))+(d-e)/f*g" + "+(((d-e)/f)*g)+f1(a,b~,c,~d,e~(f+g),h~!i)";
        lstInput = rScript.GetLstLexemes(strInput);
        strExpected = "a(RSyntacticName), ::(ROperatorBinary), b(RSyntacticName), " + ":::(ROperatorBinary), ba(RSyntacticName), $(ROperatorBinary), " + "c(RSyntacticName), @(ROperatorBinary), d(RSyntacticName), ^(ROperatorBinary), " + "e(RSyntacticName), :(ROperatorBinary), ea(RSyntacticName), %%(ROperatorBinary), " + "f(RSyntacticName), %/%(ROperatorBinary), g(RSyntacticName), %*%(ROperatorBinary), " + "h(RSyntacticName), %o%(ROperatorBinary), i(RSyntacticName), %x%(ROperatorBinary), " + "j(RSyntacticName), %in%(ROperatorBinary), k(RSyntacticName), /(ROperatorBinary), " + "l(RSyntacticName), *(ROperatorBinary), m(RSyntacticName), +(ROperatorBinary), " + "n(RSyntacticName), -(ROperatorBinary), o(RSyntacticName), <(ROperatorBinary), " + "p(RSyntacticName), >(ROperatorBinary), q(RSyntacticName), <=(ROperatorBinary), " + "r(RSyntacticName), >=(ROperatorBinary), s(RSyntacticName), ==(ROperatorBinary), " + "t(RSyntacticName), !=(ROperatorBinary), u(RSyntacticName), !(ROperatorBinary), " + "v(RSyntacticName), &(ROperatorBinary), wx(RSyntacticName), &&(ROperatorBinary), " + "y(RSyntacticName), |(ROperatorBinary), z(RSyntacticName), ||(ROperatorBinary), " + "a2(RSyntacticName), ~(ROperatorBinary), 2b(RSyntacticName), ->(ROperatorBinary), " + "c0a(RSyntacticName), ->>(ROperatorBinary), d0123456789a(RSyntacticName), " + "<-(ROperatorBinary), 1234567890(RSyntacticName), <<-(ROperatorBinary), " + "e0a1b2(RSyntacticName), =(ROperatorBinary), 345f6789(RSyntacticName), " + "+(ROperatorBinary), a(RSyntacticName), /(ROperatorBinary), ((RBracket), " + "b(RSyntacticName), )(RBracket), *(ROperatorBinary), ((RBracket), ((RBracket), " + "c(RSyntacticName), )(RBracket), )(RBracket), +(ROperatorBinary), ((RBracket), " + "d(RSyntacticName), -(ROperatorBinary), e(RSyntacticName), )(RBracket), " + "/(ROperatorBinary), f(RSyntacticName), *(ROperatorBinary), g(RSyntacticName), " + "+(ROperatorBinary), ((RBracket), ((RBracket), ((RBracket), d(RSyntacticName), " + "-(ROperatorBinary), e(RSyntacticName), )(RBracket), /(ROperatorBinary), " + "f(RSyntacticName), )(RBracket), *(ROperatorBinary), g(RSyntacticName), " + ")(RBracket), +(ROperatorBinary), f1(RFunctionName), ((RBracket), " + "a(RSyntacticName), ,(RSeparator), b(RSyntacticName), ~(ROperatorUnaryLeft), " + ",(RSeparator), c(RSyntacticName), ,(RSeparator), ~(ROperatorUnaryRight), " + "d(RSyntacticName), ,(RSeparator), e(RSyntacticName), ~(ROperatorBinary), " + "((RBracket), f(RSyntacticName), +(ROperatorBinary), g(RSyntacticName), " + ")(RBracket), ,(RSeparator), h(RSyntacticName), ~(ROperatorBinary), " + "!(ROperatorUnaryRight), i(RSyntacticName), )(RBracket), ";
        strActual = GetLstTokensAsString(rScript.GetLstTokens(lstInput));
        Assert.Equal(strExpected, strActual);

        // test token list - user-defined operators
        strInput = ".a%%a_2ab%/%ac%*%aba%o%aba2%x%abaa%in%abaaa%>%abcdefg%mydefinedoperator%hijklmnopqrstuvwxyz";
        lstInput = rScript.GetLstLexemes(strInput);
        strExpected = ".a(RSyntacticName), %%(ROperatorBinary), " + "a_2ab(RSyntacticName), %/%(ROperatorBinary), " + "ac(RSyntacticName), %*%(ROperatorBinary), " + "aba(RSyntacticName), %o%(ROperatorBinary), " + "aba2(RSyntacticName), %x%(ROperatorBinary), " + "abaa(RSyntacticName), %in%(ROperatorBinary), " + "abaaa(RSyntacticName), %>%(ROperatorBinary), " + "abcdefg(RSyntacticName), %mydefinedoperator%(ROperatorBinary), " + "hijklmnopqrstuvwxyz(RSyntacticName), ";
        strActual = GetLstTokensAsString(rScript.GetLstTokens(lstInput));
        Assert.Equal(strExpected, strActual);

        // test token list - ROperatorBracket
        strInput = "a[1]-b[c(d)+e]/f(g[2],h[3],i[4]*j[5])-k[l[m[6]]];df[[\"a\"]];lst[[\"a\"]]" + "[[\"b\"]]n[[[o][p]]]";
        lstInput = rScript.GetLstLexemes(strInput);
        strExpected = "a(RSyntacticName), [(ROperatorBracket), 1(RSyntacticName), " + "](ROperatorBracket), -(ROperatorBinary), b(RSyntacticName), [(ROperatorBracket), " + "c(RFunctionName), ((RBracket), d(RSyntacticName), )(RBracket), +(ROperatorBinary), " + "e(RSyntacticName), ](ROperatorBracket), /(ROperatorBinary), f(RFunctionName), " + "((RBracket), g(RSyntacticName), [(ROperatorBracket), 2(RSyntacticName), " + "](ROperatorBracket), ,(RSeparator), h(RSyntacticName), [(ROperatorBracket), " + "3(RSyntacticName), ](ROperatorBracket), ,(RSeparator), i(RSyntacticName), " + "[(ROperatorBracket), 4(RSyntacticName), ](ROperatorBracket), *(ROperatorBinary), " + "j(RSyntacticName), [(ROperatorBracket), 5(RSyntacticName), ](ROperatorBracket), " + ")(RBracket), -(ROperatorBinary), k(RSyntacticName), [(ROperatorBracket), " + "l(RSyntacticName), [(ROperatorBracket), m(RSyntacticName), [(ROperatorBracket), " + "6(RSyntacticName), ](ROperatorBracket), ](ROperatorBracket), ](ROperatorBracket), " + ";(REndStatement), df(RSyntacticName), [[(ROperatorBracket), \"a\"(RStringLiteral), " + "]](ROperatorBracket), ;(REndStatement), lst(RSyntacticName), [[(ROperatorBracket), " + "\"a\"(RStringLiteral), ]](ROperatorBracket), [[(ROperatorBracket), " + "\"b\"(RStringLiteral), ]](ROperatorBracket), n(RSyntacticName), [[(ROperatorBracket), " + "[(ROperatorBracket), o(RSyntacticName), ](ROperatorBracket), [(ROperatorBracket), " + "p(RSyntacticName), ](ROperatorBracket), ]](ROperatorBracket), ";
        strActual = GetLstTokensAsString(rScript.GetLstTokens(lstInput));
        Assert.Equal(strExpected, strActual);

        // test token list - end statement excluding key words
        strInput = "complete" + "\n" + "complete()" + "\n" + "complete(a[b],c[[d]])" + "\n" + "complete #" + "\n" + "complete " + "\n" + "complete + !e" + "\n" + "complete() -f" + "\n" + "complete() * g~" + "\n" + "incomplete::" + "\n" + "\n" + "incomplete::h i::: " + "\n" + "ia" + "\n" + "incomplete %>% #comment" + "\n" + "ib" + "\n" + "incomplete(" + "\n" + "ic)" + "\n" + "incomplete()[id " + "\n" + "]" + "\n" + "incomplete([[j[k]]]  " + "\n" + ")" + "\n" + "incomplete >= " + "\n" + "  #comment " + "\n" + "\n" + "l" + "\n";
        lstInput = rScript.GetLstLexemes(strInput);
        strExpected = "complete(RSyntacticName), " + "\n" + "(REndStatement), complete(RFunctionName), ((RBracket), )(RBracket), " + "\n" + "(REndStatement), complete(RFunctionName), ((RBracket), a(RSyntacticName), " + "[(ROperatorBracket), b(RSyntacticName), ](ROperatorBracket), ,(RSeparator), " + "c(RSyntacticName), [[(ROperatorBracket), d(RSyntacticName), ]](ROperatorBracket), )(RBracket), " + "\n" + "(REndStatement), complete(RSyntacticName),  (RSpace), #(RComment), " + "\n" + "(REndStatement), complete(RSyntacticName),  (RSpace), " + "\n" + "(REndStatement), complete(RSyntacticName),  (RSpace), +(ROperatorBinary), " + " (RSpace), !(ROperatorUnaryRight), e(RSyntacticName), " + "\n" + "(REndStatement), complete(RFunctionName), ((RBracket), )(RBracket), " + " (RSpace), -(ROperatorBinary), f(RSyntacticName), " + "\n" + "(REndStatement), complete(RFunctionName), ((RBracket), )(RBracket), " + " (RSpace), *(ROperatorBinary),  (RSpace), g(RSyntacticName), ~(ROperatorUnaryLeft), " + "\n" + "(REndStatement), incomplete(RSyntacticName), ::(ROperatorBinary), " + "\n" + "(RNewLine), " + "\n" + "(RNewLine), incomplete(RSyntacticName), ::(ROperatorBinary), " + "h(RSyntacticName),  (RSpace), i(RSyntacticName), :::(ROperatorBinary),  (RSpace), " + "\n" + "(RNewLine), ia(RSyntacticName), " + "\n" + "(REndStatement), incomplete(RSyntacticName),  (RSpace), %>%(ROperatorBinary), " + " (RSpace), #comment(RComment), " + "\n" + "(RNewLine), ib(RSyntacticName), " + "\n" + "(REndStatement), incomplete(RFunctionName), ((RBracket), " + "\n" + "(RNewLine), ic(RSyntacticName), )(RBracket), " + "\n" + "(REndStatement), incomplete(RFunctionName), ((RBracket), )(RBracket), " + "[(ROperatorBracket), id(RSyntacticName),  (RSpace), " + "\n" + "(RNewLine), ](ROperatorBracket), " + "\n" + "(REndStatement), incomplete(RFunctionName), ((RBracket), [[(ROperatorBracket), " + "j(RSyntacticName), [(ROperatorBracket), k(RSyntacticName), ](ROperatorBracket), " + "]](ROperatorBracket),   (RSpace), " + "\n" + "(RNewLine), )(RBracket), " + "\n" + "(REndStatement), incomplete(RSyntacticName),  (RSpace), >=(ROperatorBinary),  (RSpace), " + "\n" + "(RNewLine),   (RSpace), #comment (RComment), " + "\n" + "(RNewLine), " + "\n" + "(RNewLine), l(RSyntacticName), " + "\n" + "(REndScript), ";
        strActual = GetLstTokensAsString(rScript.GetLstTokens(lstInput));
        Assert.Equal(strExpected, strActual);

        // test token list - key words with curly brackets
        // TODO uncomment
        // strInput = "if(x > 10){" & vbLf & "    fn1(paste(x, ""is greater than 10""))" & vbLf & "}" &
        // vbLf & "else" & vbLf & "{" & vbLf & "    fn2(paste(x, ""Is less than 10""))" &
        // vbLf & "} " &
        // vbLf & "while (val <= 5 )" & vbLf & "{" & vbLf & "    # statements" &
        // vbLf & "    fn3(val)" & vbLf & "    val = val + 1" & vbLf & "}" &
        // vbLf & "repeat" & vbLf & "{" & vbLf & "    if(val > 5) break" & vbLf & "}" &
        // vbLf & "for (val in 1:5) {}" &
        // vbLf & "evenOdd = function(x){" &
        // vbLf & "if(x %% 2 == 0)" & vbLf & "    return(""even"")" & vbLf & "else" &
        // vbLf & "    return(""odd"")" & vbLf & "}" &
        // vbLf & "for (i in val)" & vbLf & "{" & vbLf & "    if (i == 8)" &
        // vbLf & "        next" & vbLf & "    if(i == 5)" & vbLf & "        break" & vbLf & "}"
        // lstInput = clsRScript.GetLstLexemes(strInput)
        // strExpected =
        // "if(RKeyWord), ((RBracket), x(RSyntacticName),  (RSpace), >(ROperatorBinary), " &
        // " (RSpace), 10(RSyntacticName), )(RBracket), {(RBracket), " &
        // vbLf & "(REndStatement),     (RSpace), fn1(RFunctionName), ((RBracket), " &
        // "paste(RFunctionName), ((RBracket), x(RSyntacticName), ,(RSeparator),  (RSpace), " &
        // """is greater than 10""(RStringLiteral), )(RBracket), )(RBracket), " &
        // vbLf & "(REndStatement), }(REndScript), " &
        // vbLf & "(RNewLine), else(RKeyWord), " &
        // vbLf & "(RNewLine), {(RBracket), " &
        // vbLf & "(REndStatement),     (RSpace), fn2(RFunctionName), ((RBracket), " &
        // "paste(RFunctionName), ((RBracket), x(RSyntacticName), ,(RSeparator), " &
        // " (RSpace), ""Is less than 10""(RStringLiteral), )(RBracket), )(RBracket), " &
        // vbLf & "(REndStatement), }(REndScript),  (RSpace), " &
        // vbLf & "(RNewLine), " &
        // "while(RKeyWord),  (RSpace), ((RBracket), val(RSyntacticName),  (RSpace), " &
        // "<=(ROperatorBinary),  (RSpace), 5(RSyntacticName),  (RSpace), )(RBracket), " &
        // vbLf & "(RNewLine), {(RBracket), " &
        // vbLf & "(REndStatement),     (RSpace), # statements(RComment), " &
        // vbLf & "(RNewLine),     (RSpace), fn3(RFunctionName), ((RBracket), " &
        // "val(RSyntacticName), )(RBracket), " &
        // vbLf & "(REndStatement),     (RSpace), val(RSyntacticName),  (RSpace), =(ROperatorBinary), " &
        // " (RSpace), val(RSyntacticName),  (RSpace), +(ROperatorBinary),  (RSpace), 1(RSyntacticName), " &
        // vbLf & "(REndStatement), }(REndScript), " &
        // vbLf & "(RNewLine), " &
        // "repeat(RKeyWord), " &
        // vbLf & "(RNewLine), {(RBracket), " &
        // vbLf & "(REndStatement),     (RSpace), if(RKeyWord), ((RBracket), val(RSyntacticName),  (RSpace), " &
        // ">(ROperatorBinary),  (RSpace), 5(RSyntacticName), )(RBracket),  (RSpace), break(RKeyWord), " &
        // vbLf & "(REndStatement), }(REndScript), " &
        // vbLf & "(REndStatement), " &
        // "for(RKeyWord),  (RSpace), ((RBracket), val(RSyntacticName),  (RSpace), in(RKeyWord), " &
        // " (RSpace), 1(RSyntacticName), :(ROperatorBinary), 5(RSyntacticName), )(RBracket), " &
        // " (RSpace), {(RBracket), }(REndScript), " &
        // vbLf & "(REndStatement), evenOdd(RSyntacticName),  (RSpace), =(ROperatorBinary),  (RSpace), " &
        // "function(RKeyWord), ((RBracket), x(RSyntacticName), )(RBracket), {(RBracket), " &
        // vbLf & "(REndStatement), if(RKeyWord), ((RBracket), x(RSyntacticName),  (RSpace), " &
        // "%%(ROperatorBinary),  (RSpace), 2(RSyntacticName),  (RSpace), ==(ROperatorBinary), " &
        // " (RSpace), 0(RSyntacticName), )(RBracket), " &
        // vbLf & "(RNewLine),     (RSpace), return(RFunctionName), ((RBracket), " &
        // """even""(RStringLiteral), )(RBracket), " &
        // vbLf & "(REndScript), else(RKeyWord), " &
        // vbLf & "(RNewLine),     (RSpace), return(RFunctionName), ((RBracket), " &
        // """odd""(RStringLiteral), )(RBracket), " &
        // vbLf & "(REndScript), }(REndScript), " &
        // vbLf & "(REndStatement), for(RKeyWord),  (RSpace), ((RBracket), i(RSyntacticName), " &
        // " (RSpace), in(RKeyWord),  (RSpace), val(RSyntacticName), )(RBracket), " &
        // vbLf & "(RNewLine), {(RBracket), " &
        // vbLf & "(REndStatement),     (RSpace), if(RKeyWord),  (RSpace), ((RBracket), " &
        // "i(RSyntacticName),  (RSpace), ==(ROperatorBinary),  (RSpace), 8(RSyntacticName), " &
        // ")(RBracket), " &
        // vbLf & "(RNewLine),         (RSpace), next(RKeyWord), " &
        // vbLf & "(REndScript),     (RSpace), if(RKeyWord), ((RBracket), i(RSyntacticName), " &
        // " (RSpace), ==(ROperatorBinary),  (RSpace), 5(RSyntacticName), )(RBracket), " &
        // vbLf & "(RNewLine),         (RSpace), break(RKeyWord), " &
        // vbLf & "(REndScript), }(REndScript), "
        // strActual = GetLstTokensAsString(clsRScript.GetLstTokens(lstInput))
        // Assert.Equal(strExpected, strActual)

        // test token list - if statement
        // TODO uncomment
        // strInput = "if(a<b){c}" &
        // vbLf & "if(d<=e){f}" &
        // vbLf & "if(g==h) { #1" &
        // vbLf & " i } #2" &
        // vbLf & " if (j >= k)" &
        // vbLf & "{" &
        // vbLf & "l   #3  " &
        // vbLf & "}" &
        // vbLf & "if (m)" & vbLf & "#4" &
        // vbLf & "  n+" & vbLf & "  o  #5" &
        // vbLf & "if(p!=q)" &
        // vbLf & "{" &
        // vbLf & "incomplete()[id " & vbLf & "]" &
        // vbLf & "incomplete([[j[k]]]  " & vbLf & ")" &
        // vbLf & "}" & vbLf
        // lstInput = clsRScript.GetLstLexemes(strInput)
        // strExpected =
        // "if(RKeyWord), ((RBracket), a(RSyntacticName), <(ROperatorBinary), b(RSyntacticName), )(RBracket), {(RBracket), c(RSyntacticName), }(REndScript), " &
        // vbLf & "(REndStatement), if(RKeyWord), ((RBracket), d(RSyntacticName), <=(ROperatorBinary), e(RSyntacticName), )(RBracket), {(RBracket), f(RSyntacticName), }(REndScript), " &
        // vbLf & "(REndStatement), if(RKeyWord), ((RBracket), g(RSyntacticName), ==(ROperatorBinary), h(RSyntacticName), )(RBracket),  (RSpace), {(RBracket),  (RSpace), #1(RComment), " &
        // vbLf & "(REndStatement),  (RSpace), i(RSyntacticName),  (RSpace), }(REndScript),  (RSpace), #2(RComment), " &
        // vbLf & "(REndStatement),  (RSpace), if(RKeyWord),  (RSpace), ((RBracket), j(RSyntacticName),  (RSpace), >=(ROperatorBinary),  (RSpace), k(RSyntacticName), )(RBracket), " &
        // vbLf & "(RNewLine), {(RBracket), " &
        // vbLf & "(REndStatement), l(RSyntacticName),    (RSpace), #3  (RComment), " &
        // vbLf & "(REndStatement), }(REndScript), " &
        // vbLf & "(REndStatement), if(RKeyWord),  (RSpace), ((RBracket), m(RSyntacticName), )(RBracket), " &
        // vbLf & "(RNewLine), #4(RComment), " &
        // vbLf & "(RNewLine),   (RSpace), n(RSyntacticName), +(ROperatorBinary), " &
        // vbLf & "(RNewLine),   (RSpace), o(RSyntacticName),   (RSpace), #5(RComment), " &
        // vbLf & "(REndScript), if(RKeyWord), ((RBracket), p(RSyntacticName), !=(ROperatorBinary), q(RSyntacticName), )(RBracket), " &
        // vbLf & "(RNewLine), {(RBracket), " &
        // vbLf & "(REndStatement), incomplete(RFunctionName), ((RBracket), )(RBracket), [(ROperatorBracket), id(RSyntacticName),  (RSpace), " &
        // vbLf & "(RNewLine), ](ROperatorBracket), " &
        // vbLf & "(REndStatement), incomplete(RFunctionName), ((RBracket), [[(ROperatorBracket), j(RSyntacticName), [(ROperatorBracket), k(RSyntacticName), ](ROperatorBracket), ]](ROperatorBracket),   (RSpace), " &
        // vbLf & "(RNewLine), )(RBracket), " &
        // vbLf & "(REndStatement), }(REndScript), " &
        // vbLf & "(REndStatement), "
        // strActual = GetLstTokensAsString(clsRScript.GetLstTokens(lstInput))
        // Assert.Equal(strExpected, strActual)

        // "f11(a,b)" & vbLf &
        // "f12(a,b,c)" & vbLf &
        // "f1(f2(),f3(a),f4(b=1),f5(c=2,3),f6(4,d=5),f7(,),f8(,,),f9(,,,),f10(a,,))" & vbLf &
        // "f0(f1(), f2(a), f3(f4()), f5(f6(f7(b))))" & vbLf &
        // "a/(b)*((c))+(d-e)/f*g+(((d-e)/f)*g)"
        // 
        // test token list - nested key words with no { TODO doesn't pass yet, see my notes of 03/01/21 for an idea on how to fix
        // strInput =
        // "for(a in b)" &
        // vbLf & "    while(c<d)" &
        // vbLf & "        repeat" &
        // vbLf & "            if(e=f)" &
        // vbLf & "                break" &
        // vbLf & "            else" &
        // vbLf & "                next" &
        // vbLf & "if (function(fn1(g,fn2=function(h)fn3(i/sum(j)*100)))))" &
        // vbLf & "    return(k)" & vbLf
        // lstInput = clsRScript.GetLstLexemes(strInput)
        // strExpected =
        // ""
        // strActual = GetLstTokensAsString(clsRScript.GetLstTokens(lstInput))
        // Assert.Equal(strExpected, strActual)

    }

    [Fact]
    public void TestGetAsExecutableScript()
    {
        string strInput, strActual;
        OrderedDictionary dctRStatements;

        strInput = "x[3:5]<-13:15;names(x)[3]<-\" Three\"" + "\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal(strInput, strActual);
        dctRStatements = new clsRScript(strInput).dctRStatements;
        Assert.Equal((UInt32)2, (UInt32)dctRStatements.Count);
        Assert.Equal((UInt32)0, dctRStatements.Cast<DictionaryEntry>().ElementAt(0).Key);
        Assert.Equal((UInt32)14, dctRStatements.Cast<DictionaryEntry>().ElementAt(1).Key);

        strInput = " f1(f2(),f3(a),f4(b=1),f5(c=2,3),f6(4,d=5),f7(,),f8(,,),f9(,,,),f10(a,,))" + "\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal(" f1(f2(),f3(a),f4(b =1),f5(c =2,3),f6(4,d =5),f7(,),f8(,,),f9(,,,),f10(a,,))" + "\n", strActual);
        dctRStatements = new clsRScript(strInput).dctRStatements;
        Assert.Single(dctRStatements);
        Assert.Equal((UInt32)0, dctRStatements.Cast<DictionaryEntry>().ElementAt(0).Key);

        strInput = "f0(f1(),f2(a),f3(f4()),f5(f6(f7(b))))" + "\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal(strInput, strActual);

        strInput = "f0(o4a=o4b,o4c=(o8a+o8b)*(o8c-o8d),o4d=f4a(o6e=o6f,o6g=o6h))" + "\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal("f0(o4a =o4b,o4c =(o8a+o8b)*(o8c-o8d),o4d =f4a(o6e =o6f,o6g =o6h))" + "\n", strActual);

        strInput = "a+b+c" + "\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal(strInput, strActual);

        strInput = "2+1-10/5*3" + "\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal(strInput, strActual);

        strInput = "1+2-3*10/5" + "\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal(strInput, strActual);

        strInput = "(a-b)*(c+d)" + "\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal(strInput, strActual);

        strInput = "a/(b)*((c))+(d-e)/f*g+(((d-e)/f)*g)" + "\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal("a/(b)*(c)+(d-e)/f*g+(((d-e)/f)*g)" + "\n", strActual);

        strInput = "var1<-pkg1::var2" + "\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal(strInput, strActual);

        strInput = "var1<-pkg1::obj1$obj2$var2" + "\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal(strInput, strActual);

        strInput = "var1<-pkg1::obj1$fun1(para1,para2)" + "\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal(strInput, strActual);

        strInput = "a<-b::c(d)+e" + "\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal(strInput, strActual);

        strInput = "f1(~a,b~,-c,+d,e~(f+g),!h,i^(-j),k+(~l),m~(~n),o/-p,q*+r)" + "\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal(strInput, strActual);



        strInput = "a[1]-b[c(d)+e]/f(g[2],h[3],i[4]*j[5])-k[l[m[6]]]" + "\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal(strInput, strActual);

        strInput = "a[[1]]-b[[c(d)+e]]/f(g[[2]],h[[3]],i[[4]]*j[[5]])-k[[l[[m[6]]]]]" + "\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal(strInput, strActual);

        strInput = "df[[\"a\"]]" + "\n" + "lst[[\"a\"]][[\"b\"]]" + "\n"; // same as 'df$a' and 'lst$a$b'
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal(strInput, strActual);
        dctRStatements = new clsRScript(strInput).dctRStatements;
        Assert.Equal((UInt32)2, (UInt32)dctRStatements.Count);
        Assert.Equal((UInt32)0, dctRStatements.Cast<DictionaryEntry>().ElementAt(0).Key);
        Assert.Equal((UInt32)10, dctRStatements.Cast<DictionaryEntry>().ElementAt(1).Key);

        strInput = "x<-\"a\";df[x]" + "\n"; // same as 'df$a' and 'lst$a$b'
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal(strInput, strActual);
        dctRStatements = new clsRScript(strInput).dctRStatements;
        Assert.Equal((UInt32)2, (UInt32)dctRStatements.Count);
        Assert.Equal((UInt32)0, dctRStatements.Cast<DictionaryEntry>().ElementAt(0).Key);
        Assert.Equal((UInt32)7, dctRStatements.Cast<DictionaryEntry>().ElementAt(1).Key);

        strInput = "df<-data.frame(x = 1:10, y = 11:20, z = letters[1:10])" + "\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal(strInput, strActual);

        strInput = "x[3:5]<-13:15;names(x)[3]<-\"Three\"" + "\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal(strInput, strActual);
        dctRStatements = new clsRScript(strInput).dctRStatements;
        Assert.Equal((UInt32)2, (UInt32)dctRStatements.Count);
        Assert.Equal((UInt32)0, dctRStatements.Cast<DictionaryEntry>().ElementAt(0).Key);
        Assert.Equal((UInt32)14, dctRStatements.Cast<DictionaryEntry>().ElementAt(1).Key);

        strInput = "x[3:5]<-13:15;" + "\n" + "names(x)[3]<-\"Three\"" + "\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal(strInput, strActual);
        dctRStatements = new clsRScript(strInput).dctRStatements;
        Assert.Equal((UInt32)3, (UInt32)dctRStatements.Count);
        Assert.Equal((UInt32)0, dctRStatements.Cast<DictionaryEntry>().ElementAt(0).Key);
        Assert.Equal((UInt32)14, dctRStatements.Cast<DictionaryEntry>().ElementAt(1).Key);
        Assert.Equal((UInt32)15, dctRStatements.Cast<DictionaryEntry>().ElementAt(2).Key);

        strInput = "x[3:5]<-13:15;" + "\r\n" + "names(x)[3]<-\"Three\"" + "\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal("x[3:5]<-13:15;" + "\n" + "names(x)[3]<-\"Three\"" + "\n", strActual);
        dctRStatements = new clsRScript(strInput).dctRStatements;
        Assert.Equal((UInt32)3, (UInt32)dctRStatements.Count);
        Assert.Equal((UInt32)0, dctRStatements.Cast<DictionaryEntry>().ElementAt(0).Key);
        Assert.Equal((UInt32)14, dctRStatements.Cast<DictionaryEntry>().ElementAt(1).Key);
        Assert.Equal((UInt32)16, dctRStatements.Cast<DictionaryEntry>().ElementAt(2).Key);

        strInput = "x[3:5]<-13:15;#comment" + "\n" + "names(x)[3]<-\"Three\"" + "\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal(strInput, strActual);
        dctRStatements = new clsRScript(strInput).dctRStatements;
        Assert.Equal((UInt32)3, (UInt32)dctRStatements.Count);
        Assert.Equal((UInt32)0, dctRStatements.Cast<DictionaryEntry>().ElementAt(0).Key);
        Assert.Equal((UInt32)14, dctRStatements.Cast<DictionaryEntry>().ElementAt(1).Key);
        Assert.Equal((UInt32)23, dctRStatements.Cast<DictionaryEntry>().ElementAt(2).Key);

        strInput = "a[]" + "\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal(strInput, strActual);

        strInput = "a[,]" + "\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal(strInput, strActual);

        strInput = "a[,,]" + "\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal(strInput, strActual);

        strInput = "a[,,,]" + "\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal(strInput, strActual);

        strInput = "a[b,]" + "\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal(strInput, strActual);

        strInput = "a[,c]" + "\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal(strInput, strActual);

        strInput = "a[b,c]" + "\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal(strInput, strActual);

        strInput = "a[\"b\",]" + "\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal(strInput, strActual);

        strInput = "a[,\"c\",1]" + "\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal(strInput, strActual);

        strInput = "a[-1,1:2,,x<5|x>7]" + "\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal(strInput, strActual);

        strInput = " a[]#comment" + "\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal(strInput, strActual);

        strInput = "a [,]" + "\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal(strInput, strActual);

        strInput = "a[ ,,] #comment" + "\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal(strInput, strActual);

        strInput = "a[, ,,]" + "\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal(strInput, strActual);

        strInput = "a[b, ]   #comment" + "\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal("a[b,]   #comment" + "\n", strActual);

        strInput = "a [  ,   c    ]     " + "\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal("a [  ,   c]     " + "\n", strActual);

        strInput = "#comment" + "\n" + "a[b,c]" + "\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal(strInput, strActual);

        strInput = "a[ \"b\"  ,]" + "\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal(strInput, strActual);

        strInput = "a[,#comment" + "\n" + "\"c\",  1 ]" + "\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal("a[,#comment" + "\n" + "\"c\",  1]" + "\n", strActual);

        strInput = "a[ -1 , 1  :   2    ,     ,      x <  5   |    x      > 7  ]" + "\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal("a[ -1 , 1  :   2    ,     ,      x <  5   |    x      > 7]" + "\n", strActual);

        // https://github.com/lloyddewit/RScript/issues/18
        strInput = "weather[,1]<-As.Date(weather[,1],format = \"%m/%d/%Y\")" + "\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal(strInput, strActual);

        strInput = " weather  [   ,  #comment" + "\n" + "  1     ] <-  As.Date   (weather     [#comment" + "\n" + " ,  1   ]    ,    format =  \"%m/%d/%Y\"    )     " + "\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal(" weather  [   ,  #comment" + "\n" + "  1] <-  As.Date(weather     [#comment" + "\n" + " ,  1],    format =  \"%m/%d/%Y\")     " + "\n", strActual);

        strInput = "dat <- dat[order(dat$tree, dat$dir), ]" + "\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal("dat <- dat[order(dat$tree, dat$dir),]" + "\n", strActual);

        // https://github.com/africanmathsinitiative/R-Instat/pull/8551
        strInput = "d22 <- d22[order(d22$tree, d22$day),]" + "\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal(strInput, strActual);

        strInput = "res <- MCA(poison[,3:8],excl =c(1,3))" + "\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal(strInput, strActual);



        strInput = "data_book$display_daily_table(data_name = \"dodoma\", climatic_element = \"rain\", " + "date_col = \"Date\", year_col = \"year\", Misscode = \"m\", monstats = c(sum = \"sum\"))" + "\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal(strInput, strActual);

        strInput = "stringr::str_split_fixed(string = date,pattern = \" - \",n = \"5 \")" + "\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal(strInput, strActual);

        strInput = "ggplot2::ggplot(data = c(sum = \"sum\"),mapping = ggplot2::aes(x = fert,y = size,colour = variety))" + "\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal(strInput, strActual);

        strInput = "last_graph<-ggplot2::ggplot(data = survey,mapping = ggplot2::aes(x = fert,y = size,colour = variety))" + "+ggplot2::geom_line()" + "+ggplot2::geom_rug(colour = \"orange\")" + "+theme_grey()" + "+ggplot2::theme(axis.text.x = ggplot2::element_text())" + "+ggplot2::facet_grid(facets = village~variety,space = \"fixed\")" + "\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal(strInput, strActual);

        strInput = "dodoma <- data_book$get_data_frame(data_name = \"dodoma\", stack_data = TRUE, measure.vars = c(\"rain\", \"tmax\", \"tmin\"), id.vars = c(\"Date\"))" + "\n" + "last_graph <- ggplot2::ggplot(data = dodoma, mapping = ggplot2::aes(x = date, y = value, colour = variable)) + ggplot2::geom_line() + " + "ggplot2::geom_rug(data = dodoma%>%filter(is.na(value)), colour = \"red\") + theme_grey() + ggplot2::theme(axis.text.x = ggplot2::element_text(), legend.position = \"none\") + " + "ggplot2::facet_wrap(scales = \"free_y\", ncol = 1, facet = ~variable) + ggplot2::xlab(NULL)" + "\n" + "data_book$add_graph(graph_name = \"last_graph\", graph = last_graph, data_name = \"dodoma\")" + "\n" + "data_book$get_graphs(data_name = \"dodoma\", graph_name = \"last_graph\")" + "\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal(strInput, strActual);
        dctRStatements = new clsRScript(strInput).dctRStatements;
        Assert.Equal((UInt32)4, (UInt32)dctRStatements.Count);
        Assert.Equal((UInt32)0, dctRStatements.Cast<DictionaryEntry>().ElementAt(0).Key);
        Assert.Equal((UInt32)139, dctRStatements.Cast<DictionaryEntry>().ElementAt(1).Key);
        Assert.Equal((UInt32)534, dctRStatements.Cast<DictionaryEntry>().ElementAt(2).Key);
        Assert.Equal((UInt32)623, dctRStatements.Cast<DictionaryEntry>().ElementAt(3).Key);

        strInput = "a->b" + "\n" + "c->>d" + "\n" + "e<-f" + "\n" + "g<<-h" + "\n" + "i=j" + "\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal(strInput, strActual);
        dctRStatements = new clsRScript(strInput).dctRStatements;
        Assert.Equal((UInt32)5, (UInt32)dctRStatements.Count);
        Assert.Equal((UInt32)0, dctRStatements.Cast<DictionaryEntry>().ElementAt(0).Key);
        Assert.Equal((UInt32)5, dctRStatements.Cast<DictionaryEntry>().ElementAt(1).Key);
        Assert.Equal((UInt32)11, dctRStatements.Cast<DictionaryEntry>().ElementAt(2).Key);
        Assert.Equal((UInt32)16, dctRStatements.Cast<DictionaryEntry>().ElementAt(3).Key);
        Assert.Equal((UInt32)22, dctRStatements.Cast<DictionaryEntry>().ElementAt(4).Key);

        strInput = "x<-df$`a b`" + "\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal(strInput, strActual);

        strInput = "names(x)<-c(\"a\",\"b\")" + "\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal(strInput, strActual);

        strInput = "a<-b" + "\r" + "c(d)" + "\r\n" + "e->>f+g" + "\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal("a<-b" + "\n" + "c(d)" + "\n" + "e->>f+g" + "\n", strActual);

        strInput = " f1(  f2(),   f3( a),  f4(  b =1))" + "\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal(strInput, strActual);

        strInput = "  f0(   o4a = o4b,  o4c =(o8a   + o8b)  *(   o8c -  o8d),   o4d = f4a(  o6e =   o6f, o6g =  o6h))" + "\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal(strInput, strActual);

        strInput = " a  /(   b)*( c)  +(   d- e)  /   f *g  +(((   d- e)  /   f)* g)" + "\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal(strInput, strActual);

        strInput = " a  +   b    +     c" + "\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal(" a  +   b  +     c" + "\n", strActual);

        strInput = " var1  <-   pkg1::obj1$obj2$var2" + "\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal(strInput, strActual);

        strInput = "    pkg ::obj1 $obj2$fn1 (a ,b=1, c    = 2 )" + "\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal("    pkg::obj1$obj2$fn1(a,b =1, c = 2)" + "\n", strActual);

        strInput = " f1(  ~   a,    b ~,  -   c,    + d,  e   ~(    f +  g),   !    h, i  ^(   -    j), k  +(   ~    l), m  ~(   ~    n), o  /   -    p, q  *   +    r)" + "\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal(strInput, strActual);

        strInput = " a  [" + "\r" + "   1" + "\n" + "] -  b   [c (  d   )+ e  ]   /f (  g   [2 ]  ,   h[ " + "\r\n" + "3  ]  " + "\n" + " ,i [  4   ]* j  [   5] )  -   k[ l  [   m[ 6  ]   ]   ]" + "\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal(" a  [" + "\r" + "   1] -  b   [c(  d)+ e]   /f(  g   [2],   h[ " + "\r\n" + "3],i [  4]* j  [   5]) -   k[ l  [   m[ 6]]]" + "\n", strActual);
        dctRStatements = new clsRScript(strInput + "x" + "\n").dctRStatements;
        Assert.Equal((UInt32)2, (UInt32)dctRStatements.Count);
        Assert.Equal((UInt32)0, dctRStatements.Cast<DictionaryEntry>().ElementAt(0).Key);
        Assert.Equal((UInt32)129, dctRStatements.Cast<DictionaryEntry>().ElementAt(1).Key);

        strInput = "#precomment1" + "\n" + " # precomment2" + "\n" + "  #  precomment3" + "\n" + @" f1(  f2(),   f3( a),  f4(  b =1))#comment1~!@#$%^&*()_[] {} \|;:',./<>?" + "\n" + "  f0(   o4a = o4b,  o4c =(o8a   + o8b)  *(   o8c -  o8d),   o4d = f4a(  o6e =   o6f, o6g =  o6h)) # comment2\",\" cbaa " + "\n" + " a  /(   b)*( c)  +(   d- e)  /   f *g  +(((   d- e)  /   f)* g)   #comment3" + "\n" + "#comment 4" + "\n" + " a  +   b  +     c" + "\n" + "\n" + "\n" + "  #comment5" + "\n" + "   # comment6 #comment7" + "\n" + "endSyntacticName" + "\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal(strInput, strActual);
        dctRStatements = new clsRScript(strInput).dctRStatements;
        Assert.Equal((UInt32)5, (UInt32)dctRStatements.Count);
        Assert.Equal((UInt32)0, dctRStatements.Cast<DictionaryEntry>().ElementAt(0).Key);
        Assert.Equal((UInt32)118, dctRStatements.Cast<DictionaryEntry>().ElementAt(1).Key);
        Assert.Equal((UInt32)236, dctRStatements.Cast<DictionaryEntry>().ElementAt(2).Key);
        Assert.Equal((UInt32)313, dctRStatements.Cast<DictionaryEntry>().ElementAt(3).Key);
        Assert.Equal((UInt32)343, dctRStatements.Cast<DictionaryEntry>().ElementAt(4).Key);

        strActual = new clsRScript(strInput).GetAsExecutableScript(false);
        Assert.Equal("f1(f2(),f3(a),f4(b =1))" + "\n" + "f0(o4a =o4b,o4c =(o8a+o8b)*(o8c-o8d),o4d =f4a(o6e =o6f,o6g =o6h))" + "\n" + "a/(b)*(c)+(d-e)/f*g+(((d-e)/f)*g)" + "\n" + "a+b+c" + "\n" + "endSyntacticName" + "\n", strActual);

        strInput = "#comment1" + "\n" + "a#comment2" + "\r" + " b #comment3" + "\r\n" + "#comment4" + "\n" + "  c  " + "\r\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal("#comment1" + "\n" + "a#comment2" + "\n" + " b #comment3" + "\n" + "#comment4" + "\n" + "  c  " + "\n", strActual);
        dctRStatements = new clsRScript(strInput).dctRStatements;
        Assert.Equal((UInt32)3, (UInt32)dctRStatements.Count);
        Assert.Equal((UInt32)0, dctRStatements.Cast<DictionaryEntry>().ElementAt(0).Key);
        Assert.Equal((UInt32)21, dctRStatements.Cast<DictionaryEntry>().ElementAt(1).Key);
        Assert.Equal((UInt32)35, dctRStatements.Cast<DictionaryEntry>().ElementAt(2).Key);

        strActual = new clsRScript(strInput).GetAsExecutableScript(false);
        Assert.Equal("a" + "\n" + "b" + "\n" + "c" + "\n", strActual);

        strInput = "#not ignored comment";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal(strInput + "\n", strActual);
        dctRStatements = new clsRScript(strInput).dctRStatements;
        Assert.Equal((UInt32)1, (UInt32)dctRStatements.Count);

        strActual = new clsRScript(strInput).GetAsExecutableScript(false);
        Assert.Equal("\n", strActual);
        dctRStatements = new clsRScript(strActual).dctRStatements;
        Assert.Equal((UInt32)1, (UInt32)dctRStatements.Count);

        strInput = "#not ignored comment" + "\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal(strInput, strActual);
        dctRStatements = new clsRScript(strInput).dctRStatements;
        Assert.Equal((UInt32)1, (UInt32)dctRStatements.Count);

        strActual = new clsRScript(strInput).GetAsExecutableScript(false);
        Assert.Equal("\n", strActual);

        strInput = "f1()" + "\n" + "# not ignored comment" + "\r\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal(strInput, strActual);
        dctRStatements = new clsRScript(strInput).dctRStatements;
        Assert.Equal((UInt32)2, (UInt32)dctRStatements.Count);
        Assert.Equal((UInt32)0, dctRStatements.Cast<DictionaryEntry>().ElementAt(0).Key);
        Assert.Equal((UInt32)5, dctRStatements.Cast<DictionaryEntry>().ElementAt(1).Key);

        strActual = new clsRScript(strInput).GetAsExecutableScript(false);
        Assert.Equal("f1()" + "\n" + "\n", strActual);

        strInput = "f1()" + "\n" + "# not ignored comment" + "\n" + "# not ignored comment2" + "\r" + " " + "\r\n" + "# not ignored comment3";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal(strInput + "\n", strActual);
        dctRStatements = new clsRScript(strInput).dctRStatements;
        Assert.Equal((UInt32)2, (UInt32)dctRStatements.Count);
        Assert.Equal((UInt32)0, dctRStatements.Cast<DictionaryEntry>().ElementAt(0).Key);
        Assert.Equal((UInt32)5, dctRStatements.Cast<DictionaryEntry>().ElementAt(1).Key);

        // issue lloyddewit/rscript#20
        strInput = "# Code run from Script Window (all text)" + Environment.NewLine + "1";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal(strInput + "\n", strActual);

        strInput = "\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal("\n", strActual);
        dctRStatements = new clsRScript(strInput).dctRStatements;
        Assert.Equal((UInt32)1, (UInt32)dctRStatements.Count);

        strActual = new clsRScript(strInput).GetAsExecutableScript(false);
        Assert.Equal("\n", strActual);

        strInput = "";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal("", strActual);
        dctRStatements = new clsRScript(strInput).dctRStatements;
        Assert.Equal((UInt32)0, (UInt32)dctRStatements.Count);

        strActual = new clsRScript(strInput).GetAsExecutableScript(false);
        Assert.Equal("", strActual);

        strInput = null;
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal("", strActual);
        dctRStatements = new clsRScript(strInput).dctRStatements;
        Assert.Equal((UInt32)0, (UInt32)dctRStatements.Count);

        strActual = new clsRScript(strInput).GetAsExecutableScript(false);
        Assert.Equal("", strActual);

        // Test string constants that contain line breaks
        strInput = "x <- \"a" + "\n" + "\"" + "\n" + "fn1(\"bc " + "\n" + "d\", e)" + "\n" + "fn2( \"f gh" + "\n" + "\",i)" + "\n" + "x <- 'a" + "\r" + "\r" + "'" + "\n" + "fn1('bc " + "\r" + "\r" + "\r" + "\r" + "d', e)" + "\n" + "fn2( 'f gh" + "\r" + "',i)" + "\n" + "x <- `a" + "\r\n" + "`" + "\n" + "fn1(`bc " + "\r\n" + "j" + "\r\n" + "d`, e)" + "\n" + "fn2( `f gh" + "\r\n" + "kl" + "\r\n" + "mno" + "\r\n" + "`,i)" + "\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal(strInput, strActual);
        dctRStatements = new clsRScript(strInput).dctRStatements;
        Assert.Equal((UInt32)9, (UInt32)dctRStatements.Count);
        Assert.Equal((UInt32)0, dctRStatements.Cast<DictionaryEntry>().ElementAt(0).Key);
        Assert.Equal((UInt32)10, dctRStatements.Cast<DictionaryEntry>().ElementAt(1).Key);
        Assert.Equal((UInt32)26, dctRStatements.Cast<DictionaryEntry>().ElementAt(2).Key);
        Assert.Equal((UInt32)42, dctRStatements.Cast<DictionaryEntry>().ElementAt(3).Key);
        Assert.Equal((UInt32)53, dctRStatements.Cast<DictionaryEntry>().ElementAt(4).Key);
        Assert.Equal((UInt32)72, dctRStatements.Cast<DictionaryEntry>().ElementAt(5).Key);
        Assert.Equal((UInt32)88, dctRStatements.Cast<DictionaryEntry>().ElementAt(6).Key);
        Assert.Equal((UInt32)99, dctRStatements.Cast<DictionaryEntry>().ElementAt(7).Key);
        Assert.Equal((UInt32)119, dctRStatements.Cast<DictionaryEntry>().ElementAt(8).Key);

        strActual = new clsRScript(strInput).GetAsExecutableScript(false);
        Assert.Equal("x<-\"a" + "\n" + "\"" + "\n" + "fn1(\"bc " + "\n" + "d\",e)" + "\n" + "fn2(\"f gh" + "\n" + "\",i)" + "\n" + "x<-'a" + "\r" + "\r" + "'" + "\n" + "fn1('bc " + "\r" + "\r" + "\r" + "\r" + "d',e)" + "\n" + "fn2('f gh" + "\r" + "',i)" + "\n" + "x<-`a" + "\r\n" + "`" + "\n" + "fn1(`bc " + "\r\n" + "j" + "\r\n" + "d`,e)" + "\n" + "fn2(`f gh" + "\r\n" + "kl" + "\r\n" + "mno" + "\r\n" + "`,i)" + "\n", strActual);

        // https://github.com/africanmathsinitiative/R-Instat/issues/7095  
        strInput = "data_book$import_data(data_tables =list(data3 =clipr::read_clip_tbl(x =\"Category    Feature    Ease_of_Use     Operating Systems" + "\n" + "\", header =TRUE)))" + "\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal(strInput, strActual);

        // https://github.com/africanmathsinitiative/R-Instat/issues/7095  
        strInput = "Data <- data_book$get_data_frame(data_name = \"Data\")" + "\n" + "last_graph <- ggplot2::ggplot(data = Data |> dplyr::filter(rain > 0.85), mapping = ggplot2::aes(y = rain, x = make_factor(\"\")))" + " + ggplot2::geom_boxplot(varwidth = TRUE, coef = 2) + theme_grey()" + " + ggplot2::theme(axis.text.x = ggplot2::element_text(angle = 90, hjust = 1, vjust = 0.5))" + " + ggplot2::xlab(NULL) + ggplot2::facet_wrap(facets = ~ Name, drop = FALSE)" + "\n" + "data_book$add_graph(graph_name = \"last_graph\", graph = last_graph, data_name = \"Data\")" + "\n" + "data_book$get_graphs(data_name = \"Data\", graph_name = \"last_graph\")" + "\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal(strInput, strActual);
        Assert.Equal(strInput, strActual);
        dctRStatements = new clsRScript(strInput).dctRStatements;
        Assert.Equal((UInt32)4, (UInt32)dctRStatements.Count);
        Assert.Equal((UInt32)0, dctRStatements.Cast<DictionaryEntry>().ElementAt(0).Key);
        Assert.Equal((UInt32)53, dctRStatements.Cast<DictionaryEntry>().ElementAt(1).Key);
        Assert.Equal((UInt32)412, dctRStatements.Cast<DictionaryEntry>().ElementAt(2).Key);
        Assert.Equal((UInt32)499, dctRStatements.Cast<DictionaryEntry>().ElementAt(3).Key);

        // https://github.com/africanmathsinitiative/R-Instat/issues/7095  
        strInput = "ifelse(year_2 > 30, 1, 0)" + "\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal(strInput, strActual);

        // https://github.com/africanmathsinitiative/R-Instat/issues/7377
        strInput = "(year-1900)*(year<2000)+(year-2000)*(year>1999)" + "\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal(strInput, strActual);

        // Test string constants that have quotes embedded inside
        strInput = @"a("""", ""\""\"""", ""b"", ""c(\""d\"")"", ""'"", ""''"", ""'e'"", ""`"", ""``"", ""`f`"")" + "\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal(strInput, strActual);

        strInput = @"a('', '\'\'', 'b', 'c(\'d\')', '""', '""""', '""e""', '`', '``', '`f`')" + "\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal(strInput, strActual);

        strInput = @"a(``, `\`\``, `b`, `c(\`d\`)`, `""`, `""""`, `""e""`, `'`, `''`, `'f'`)" + "\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal(strInput, strActual);

        strInput = "x<-\"she said 'hello'\"" + "\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal(strInput, strActual);

        strInput = "read_clip_tbl(x = \"Ease_of_Use" + "\t" + @"Hides R by default to prevent \""code shock\""" + "\t" + "  1\", header = TRUE)" + "\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal(strInput, strActual);

        // issue lloyddewit/rscript#17
        strInput = "?log" + "\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal(strInput, strActual);

        // issue lloyddewit/rscript#21
        strInput = "?a" + "\n" + "? b" + "\n" + " +  c" + "\n" + "  -   d +#comment1" + "\n" + "(!e) - #comment2" + "\n" + "(~f) +" + "\n" + "(+g) - " + "\n" + "(-h)" + "\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal("?a" + "\n" + "? b" + "\n" + " +  c" + "\n" + "  -   d +(!e) -(~f) +(+g) -(-h)" + "\n", strActual);
        dctRStatements = new clsRScript(strInput).dctRStatements;
        Assert.Equal((UInt32)4, (UInt32)dctRStatements.Count);

        // issue lloyddewit/rscript#32
        strInput = "??log" + "\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal(strInput, strActual);

        strInput = "??a" + "\n" + "?? b" + "\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal(strInput, strActual);

        // issue lloyddewit/rscript#16
        strInput = "\"a\"+\"b\"" + "\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal(strInput, strActual);

        strInput = "  tfrmt(" + "\n" + "  # specify columns in the data" + "\n" + "  group = c(rowlbl1, grp)," + "\n" + "  label = rowlbl2," + "\n" + "  column = column, " + "\n" + "  param = param," + "\n" + "  value = value," + "\n" + "  sorting_cols = c(ord1, ord2)," + "\n" + "  # specify value formatting " + "\n" + "  body_plan = body_plan(" + "\n" + "  frmt_structure(group_val = \".default\", label_val = \".default\", frmt_combine(\"{n} ({pct} %)\"," + "\n" + "                                                                    n = frmt(\"xxx\")," + "\n" + "                                                                                pct = frmt(\"xx.x\")))," + "\n" + "    frmt_structure(group_val = \".default\", label_val = \"n\", frmt(\"xxx\"))," + "\n" + "    frmt_structure(group_val = \".default\", label_val = c(\"Mean\", \"Median\", \"Min\", \"Max\"), frmt(\"xxx.x\"))," + "\n" + "    frmt_structure(group_val = \".default\", label_val = \"SD\", frmt(\"xxx.xx\"))," + "\n" + "    frmt_structure(group_val = \".default\", label_val = \".default\", p = frmt_when(\">0.99\" ~ \">0.99\"," + "\n" + "                                                                                 \"<0.001\" ~ \"<0.001\"," + "\n" + "                                                                                 TRUE ~ frmt(\"x.xxx\", missing = \"\"))))) %>% " + "\n" + "  print_to_gt(data_demog) %>% " + "\n" + "  tab_options(" + "\n" + "    container.width = 900)" + "\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal(strInput, strActual);

        // issue lloyddewit/rscript#14
        // Examples from https://www.tidyverse.org/blog/2023/04/base-vs-magrittr-pipe/
        strInput = "x %>% f(1, .)" + "\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal(strInput, strActual);

        strInput = "x |> f(1, y = _)" + "\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal(strInput, strActual);

        strInput = "df %>% split(.$var)" + "\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal(strInput, strActual);

        // TODO curly brackets not yet supported
        // strInput = "df %>% {split(.$x, .$y)}" & vbLf
        // strActual = new clsRScript(strInput).GetAsExecutableScript()
        // Assert.Equal(strInput, strActual)

        strInput = "mtcars %>% .$cyl" + "\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal(strInput, strActual);


        // Examples from https://stackoverflow.com/questions/67744604/what-does-pipe-greater-than-mean-in-r
        strInput = "c(1:3, NA_real_) |> sum(na.rm = TRUE)" + "\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal(strInput, strActual);

        strInput = "split(x = iris[-5], f = iris$Species) |> lapply(min) |> Do.call(what = rbind)" + "\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal(strInput, strActual);

        strInput = "iris[iris$Sepal.Length > 7,] %>% subset(.$Species==\"virginica\")" + "\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal(strInput, strActual);

        strInput = "1:3 |> sum" + "\n";
        strActual = new clsRScript(strInput).GetAsExecutableScript();
        Assert.Equal(strInput, strActual);

    }

    private string GetLstTokensAsString(List<clsRToken> lstRTokens)
    {

        if (lstRTokens is null || lstRTokens.Count == 0)
        {
            // TODO throw exception
            return null;
        }

        string strNew = "";
        foreach (var clsRTokenNew in lstRTokens)
        {
            strNew += clsRTokenNew.strTxt + "(";
            switch (clsRTokenNew.enuToken)
            {
                case clsRToken.typToken.RSyntacticName:
                    {
                        strNew += "RSyntacticName";
                        break;
                    }
                case clsRToken.typToken.RFunctionName:
                    {
                        strNew += "RFunctionName";
                        break;
                    }
                case clsRToken.typToken.RKeyWord:
                    {
                        strNew += "RKeyWord";
                        break;
                    }
                case clsRToken.typToken.RConstantString:
                    {
                        strNew += "RStringLiteral";
                        break;
                    }
                case clsRToken.typToken.RComment:
                    {
                        strNew += "RComment";
                        break;
                    }
                case clsRToken.typToken.RSpace:
                    {
                        strNew += "RSpace";
                        break;
                    }
                case clsRToken.typToken.RBracket:
                    {
                        strNew += "RBracket";
                        break;
                    }
                case clsRToken.typToken.RSeparator:
                    {
                        strNew += "RSeparator";
                        break;
                    }
                case clsRToken.typToken.RNewLine:
                    {
                        strNew += "RNewLine";
                        break;
                    }
                case clsRToken.typToken.REndStatement:
                    {
                        strNew += "REndStatement";
                        break;
                    }
                case clsRToken.typToken.REndScript:
                    {
                        strNew += "REndScript";
                        break;
                    }
                case clsRToken.typToken.ROperatorUnaryLeft:
                    {
                        strNew += "ROperatorUnaryLeft";
                        break;
                    }
                case clsRToken.typToken.ROperatorUnaryRight:
                    {
                        strNew += "ROperatorUnaryRight";
                        break;
                    }
                case clsRToken.typToken.ROperatorBinary:
                    {
                        strNew += "ROperatorBinary";
                        break;
                    }
                case clsRToken.typToken.ROperatorBracket:
                    {
                        strNew += "ROperatorBracket";
                        break;
                    }
            }
            strNew += "), ";
        }
        return strNew;
    }

}