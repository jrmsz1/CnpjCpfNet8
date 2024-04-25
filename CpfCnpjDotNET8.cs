using System.Runtime.CompilerServices;
namespace CnpjCpfForNet8;

public static class CpfCnpjDotNET8
{

    private static readonly int[] multiplicador1Cnpj = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
    private static readonly int[] multiplicador2Cnpj = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

    
    private static readonly int[] multiplicador1Cpf = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
    private static readonly int[] multiplicador2Cpf = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

    public static bool IsValidDocument(ReadOnlySpan<char> cpfCnpj) =>
         cpfCnpj.Length <= 11 ? IsCpf(cpfCnpj) : IsCnpj(cpfCnpj);

    public static bool IsCnpj(ReadOnlySpan<char> cnpj)
    {
        // Verificar se o CNPJ possui 14 dígitos
        if (cnpj.Length != 14)
            return false;
        
        //Verifica se é um número inteiro positivo
        if (!ulong.TryParse(cnpj, out _))
            return false;

        //Posso usar vários sabores aqui V1 a V3!!!!
        if (!ValidateCnpjV3(cnpj))
            return false;

        return true;
    }

    public static bool IsCpf(ReadOnlySpan<char> cpf)
    {
        // Verificar se o CPF possui 11 dígitos
        if (cpf.Length != 11)
            return false;

        //Verifica se é um número inteiro positivo
        if (!ulong.TryParse(cpf, out _))
            return false;

        //Posso usar vários sabores aqui V1 a V3!!!!
        if (!ValidateCpfV3(cpf))
            return false;

        return true;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool ValidateCnpj(ReadOnlySpan<char> cnpj)
    {
        var soma = 0;

        // Calcula o primeiro dígito verificador
        for (int i = 0; i < 12; i++)
        {
            if (!char.IsDigit(cnpj[i]))
                return false;
            soma += (cnpj[i] - '0') * multiplicador1Cnpj[i];
        }

        //Atribuição otimista
        var digito1 = 11 - (soma % 11);
        if (soma % 11 < 2) digito1 = 0;
        if (!(cnpj[12] - '0').Equals(digito1)) return false;

        soma = 0;
        // Calcula o segundo dígito verificador
        for (int i = 0; i < 13; i++)
        {
            //Já validado no laço anterior
            if (i < 12) soma += (cnpj[i] - '0') * multiplicador2Cnpj[i];
            else
            {
                if (!char.IsDigit(cnpj[i]))
                    return false;
                soma += digito1 * multiplicador2Cnpj[i];
            }
        }

        //Valida o último dígito
        if (!char.IsDigit(cnpj[13]))
            return false;

        //Atribuição otimista
        var digito2 = 11 - (soma % 11);
        if (soma % 11 < 2) digito2 = 0;

        if ((cnpj[13] - '0').Equals(digito2)) return true;
        return false;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool ValidateCnpjV2(ReadOnlySpan<char> cnpj)
    {
        var soma = 0;
        // Calcula o primeiro dígito verificador
        var j = 6;
        for (int i = 0; i < 12; i++)
        {
            if (!char.IsDigit(cnpj[i]))
                return false;
            soma += (cnpj[i] - '0') * (j = --j == 1 ? 9 : j);
        }
        //Atribuição otimista
        var digito1 = 11 - (soma % 11);
        if (soma % 11 < 2)
            digito1 = 0;

        if (!(cnpj[12] - '0').Equals(digito1)) return false;

        soma = 0;
        // Calcula o segundo dígito verificador
        j = 7;
        for (int i = 0; i < 13; i++)
        {
            //Sequencia já validada no laço acima
            if (i < 12)
                //Já validado no laço acima
                soma += (cnpj[i] - '0') * (j = --j == 1 ? 9 : j);
            else
            {
                if (!char.IsDigit(cnpj[i]))
                    return false;
                soma += digito1 * (j = --j == 1 ? 9 : j);
            }
        }
        //Valida o último dígito
        if (!char.IsDigit(cnpj[13]))
            return false;
        //Atibuição otimista
        var digito2 = 11 - (soma % 11);
        if (soma % 11 < 2)
            digito2 = 0;

        if ((cnpj[13] - '0').Equals(digito2)) return true;
        return false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool ValidateCnpjV3(ReadOnlySpan<char> cnpj)
    {
        var digito2 = 0;
        var digito1 = 0;

        var j = 6;        
        for (int i = 0; i < 13; i++)
        {
            var dig = cnpj[i] - '0';            
            if (i < 12)
            {
                digito1 += dig * (j = --j == 1 ? 9 : j);
                digito2 += dig * (j + 1 == 10 ? 2 : j + 1);
                continue;
            }
            
            if (!dig.Equals(digito1 % 11 < 2 ? 0 : 11 - (digito1 % 11))) return false;
            digito2 += dig * 2;
            if ((cnpj[13] - '0').Equals(digito2 % 11 < 2 ? 0 : 11 - (digito2 % 11))) return true;
        }
        return false;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool ValidateCpf(ReadOnlySpan<char> cpf)
    {
        var soma = 0;
        // Calcula o primeiro dígito verificador
        for (int i = 0; i < 9; i++)
        {
            if (!char.IsDigit(cpf[i]))
                return false;
            soma += (cpf[i] - '0') * (10 - i);
        }
        //Atribuição otimista
        var digito1 = 11 - (soma % 11);
        if (soma % 11 < 2)
            digito1 = 0;
        if (!(cpf[9] - '0').Equals(digito1)) return false;

        soma = 0;
        // Calcula o segundo dígito verificador
        for (int i = 0; i < 10; i++)
        {
            if (i < 9)
                //Já validado no laço acima
                soma += (cpf[i] - '0') * (11 - i);
            else
            {
                if (!char.IsDigit(cpf[i]))
                    return false;
                soma += digito1 * (11 - i);
            }
        }
        //Valida o último dígito
        if (!char.IsDigit(cpf[10]))
            return false;
        //Atribuição otimista
        var digito2 = 11 - (soma % 11);
        if (soma % 11 < 2)
            digito2 = 0;

        if ((cpf[10] - '0').Equals(digito2)) return true;
        return false;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool ValidateCpfV2(ReadOnlySpan<char> cpf)
    {
        var soma = 0;
        // Calcula o primeiro dígito verificador
        for (int i = 0; i < 9; i++)
        {
            if (!char.IsDigit(cpf[i]))
                return false;
            soma += (cpf[i] - '0') * multiplicador1Cpf[i];
        }

        //Atribuição otimista
        var digito1 = 11 - (soma % 11);
        if (soma % 11 < 2)
            digito1 = 0;
        if (!(cpf[9] - '0').Equals(digito1)) return false;

        soma = 0;
        // Calcula o segundo dígito verificador
        for (int i = 0; i < 10; i++)
        {
            if (i < 9)
                //Já validado no laço acima
                soma += (cpf[i] - '0') * multiplicador2Cpf[i];
            else
            {
                if (!char.IsDigit(cpf[i]))
                    return false;
                soma += digito1 * multiplicador2Cpf[i];
            }
        }
        
        //Valida o último dígito
        if (!char.IsDigit(cpf[10]))
            return false;
        //Atribuição otimista
        var digito2 = 11 - (soma % 11);
        if (soma % 11 < 2)
            digito2 = 0;
        if ((cpf[10] - '0').Equals(digito2)) return true;
        return false;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool ValidateCpfV3(ReadOnlySpan<char> cpf)
    {
        var digito2 = 0;
        var digito1 = 0;                       

        for (var i = 0; i < 10; i++)
        {            
            var dig = cpf[i] - '0';
            if (i < 9)
            {
                digito2 += dig;
                digito1 += dig * (10 - i);
                continue;
            }            
            if (!dig.Equals(digito1 % 11 < 2 ? 0 : 11 - digito1 % 11)) return false;
            if ((cpf[10] - '0').Equals((digito2 + dig * 2 + digito1) % 11 < 2 ? 0 : 11 - (digito2 + dig * 2 + digito1) % 11)) return true;            
        }
        return false;
    }
}