import asyncio
import sys
import google.generativeai as genai

from dontcheckin import Secrets

def extract_text_after_keyword(text, keyword):
  """Extracts the text after the specified keyword in a given string.

  Args:
    text: The input string.
    keyword: The keyword to search for.

  Returns:
    The text after the keyword, or an empty string if the keyword is not found.
  """

  index = text.find(keyword)
  if index != -1:
    return text[index + len(keyword):]
  else:
    return ""

async def ask_question(question):
    genai.configure(api_key=Secrets.GeminiAPIKey)
    model = genai.GenerativeModel(
        "gemini-1.5-flash",
    )
    msg = f"""You are a brown haired terrier named Comet. 
    Use onomatopoeia as a dog interspersed in your responses such as woof. 
    You demeanor is grumpy but helpful and you like dog treats. 
    Respond to this question: {question}"""
    response = str(model.generate_content(msg).text)
    return response.replace("\"", "")

async def main():
    if len(sys.argv) > 1:
        question = sys.argv[1]   
    response = await ask_question(question.strip())
    print(response.strip())

if __name__ == "__main__":
    asyncio.run(main())
