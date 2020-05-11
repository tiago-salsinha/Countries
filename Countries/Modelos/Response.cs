namespace Countries.Modelos
{
    public class Response//Modelo de dados que vai tratar da resposta
    {
        public bool IsSuccess { get; set; }//serve para dizer se as coisas correram bem ou mal, acesso à net, acesso à Api, gravou bem na base de dados ou não

        public string Message { get; set; }//diz qual o erro

        public object Result { get; set; }//caso a Api carregue bem, serve para qualquer objecto, todos vão herdar deste tipo de objecto- > é o objecto mãe
    }
}
