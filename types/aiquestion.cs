namespace nehsanet_app.Types {

    
    public class AIQuestion(string question = "", string answer = "", string previousAnswer = "")
    {
        public string Question { get; set; } = question;
        public string Answer { get; set; } = answer;
        public string PreviousAnswer { get; set; } = previousAnswer;
    }
}