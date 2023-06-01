using System.Collections.Generic;
using System.Text.RegularExpressions;

string path = "C:\\TMP\\ТриПоросенка.txt";
string text = await GetText(path);
var tmpCotreg=GetFormmatingText(text);
string formattingText= tmpCotreg.Item1;
IEnumerable<IGrouping<char, char>> groupingEnum=tmpCotreg.Item2;
int nTotal = GetTotalSymbol(groupingEnum);
//через count не учитываются пробелы, по этому сделал отдельный алгоритм для их подсчета 
//без пробелов
double entopOutSpace = GetEntopiyaOutSpace(groupingEnum, nTotal);
//с пробелами
int nZeroOut = 0;
Regex regexWithoutSpace = new(@"[^а-яА-ЯёЁ]");
formattingText = regexWithoutSpace.Replace(text, string.Empty);
Console.WriteLine(formattingText);
double space, totalEnrop;
totalEnrop = GetEntropWithSpace(formattingText, nTotal, entopOutSpace, nZeroOut);
Console.WriteLine("В тексте всего символов : {0} \nВ тексте отличных от пробела сиволов : {1} \nВ тексте пробелов : {2}\nЭнтропия с пробелами: {3} ", nTotal, nZeroOut, space, totalEnrop);

async Task<string> GetText(string path)
{
     
    StreamReader reader = new(path);
    string? line;
    var text = "";
    while ((line = await reader.ReadLineAsync()) != null)
    {
        Console.WriteLine(line);
        text += line.ToLower();
    }

    return text;
}

(string, IEnumerable < IGrouping<char, char> >) GetFormmatingText(string text)
{
    Regex regexWithSpace = new(@"[^а-яА-ЯёЁ ]");
    string tmp1 = regexWithSpace.Replace(text, string.Empty);
    IEnumerable<IGrouping<char, char>> tmp = tmp1.ToCharArray().GroupBy(c => c);
    Console.WriteLine("Исходный тектс с паттерном [^а-яА-ЯёЁ ]: " + tmp1);
    return (tmp1, tmp);
}

int GetTotalSymbol(IEnumerable<IGrouping<char, char>> groupingEnum)
{
    int nTotal = 0;
    foreach (var item in groupingEnum)
    {
        nTotal += Convert.ToInt32(item.Count());
    }
    Console.WriteLine(nTotal);
    return nTotal;
}

double GetEntopiyaOutSpace(IEnumerable<IGrouping<char, char>> groupingEnum, int nTotal)
{
    double entopOutSpace = 0;
    foreach (var item in groupingEnum)
    {
        Console.WriteLine("В тексте вид буквы {1} количество '{0}'", item.Count(), item.Key);
        double tmp5 = Convert.ToInt32(item.Count());
        double tmp6 = tmp5 / nTotal;
        Console.WriteLine(tmp6);
        entopOutSpace += -tmp6 * Math.Log2(tmp6);
        Console.WriteLine(entopOutSpace);
    }

    return entopOutSpace;
}

double GetEntropWithSpace(string formattingText, int nTotal, double entopOutSpace, int nZeroOut)
{
    foreach (var item in formattingText)
    {

        nZeroOut++;
    }
    space = nTotal - nZeroOut;
    double tmp7 = space / nTotal;
    totalEnrop = entopOutSpace + (-tmp7 * Math.Log2(tmp7));
    return totalEnrop;
}