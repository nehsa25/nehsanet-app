namespace nehsanet_app.Types {

    
    public class AIQuestion(string question = "", string answer = "")
    {
        public string Question { get; set; } = question;
        public string Answer { get; set; } = answer;
    }
}