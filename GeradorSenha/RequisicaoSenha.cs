namespace GeradorSenha
{
    public class RequisicaoSenha
    {
        public bool PodeConterLetrasMaiusculas {  get; set; }
        public bool PodeConterLetrasMinusculas {  get; set; }
        public bool PodeConterLetrasMAcentuadas {  get; set; }
        public bool PodeConterNumeros {  get; set; }
        public bool PodeConterCaracteresEespeciais {  get; set; }
        public int QuantidadeCaracteres { get; set; }
    
    }
}