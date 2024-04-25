namespace CnpjCpfForNet8;

public static class GenerateDocuments
{
    public static ReadOnlySpan<char> FillArray()
    {
        char[] chars = new char[1_000_000];
        Array.Fill(chars, '0', 0, 100_000);
        Array.Fill(chars, '1', 100_000, 100_000);
        Array.Fill(chars, '2', 200_000, 100_000);
        Array.Fill(chars, '3', 300_000, 100_000);
        Array.Fill(chars, '4', 400_000, 100_000);
        Array.Fill(chars, '5', 500_000, 100_000);
        Array.Fill(chars, '6', 600_000, 100_000);
        Array.Fill(chars, '7', 700_000, 100_000);
        Array.Fill(chars, '8', 800_000, 100_000);
        Array.Fill(chars, '9', 900_000, 100_000);

        Random.Shared.Shuffle(chars);

        return chars.AsSpan();
    }
    
    public static ReadOnlySpan<char> GenerateCPFs(ReadOnlySpan<char> charsSpan, int count)
    {
        //Tratamento para IndexOutOfRange       
        return charsSpan.Slice(count + 11 < 1_000_000 ? count : (count + 11) % 11, 11);        
    }

   
    public static ReadOnlySpan<char> GenerateCNPJs(ReadOnlySpan<char> charsSpan, int count)
    {
        //Tratamento para IndexOutOfRange       
        return charsSpan.Slice(count + 14 < 1_000_000 ? count : (count + 14) % 14, 14);
    }
}