import { QuizLayout } from "../layout/QuizLayout"
import Quiz from './Quiz'
import { useContext, useState, useEffect } from "react"
import { useParams } from "react-router"
import { AppContext } from "../../services/AppContext"
import QuizResults from "./QuizResults"
import Notification from "../shared/Notification"
import QuizResultsList from "./QuizResultsList"

export default function QuizPage(){
    const {id} = useParams()
    const [quiz, setQuiz] = useState<QuizItem>()
    const [results, setResults] = useState<QuizResult[]>([])
    const [isDone, setIsDone] = useState(false)
    const [userAnswers, setAnswers] = useState<number[]>([])
    const {api} = useContext(AppContext)   
    const [errorMessage, setErrorMessage] = useState("")
    const [isError, setIsError] = useState(false)



    useEffect(() => {
        const fetchData = async () => {
          try {
            const response = await api.GetByIdAsync(id as string);
            setQuiz(response.quiz);
            setResults(response.results);
            console.log(response);
            setIsError(false);
          } catch (error) {
            // Handle errors
            console.error(error);
            setIsError(true);
            setErrorMessage("Server error: Something goes wrong");
          }
        };
    
        fetchData();
      }, []);

    function CalculateResult():number {
      var correctAnswersCount = 0;
      quiz?.questions.map((q, index)=> {
        if(q.correctAnswerIndex === userAnswers[index])
          correctAnswersCount++
      })
      return userAnswers.length/100*correctAnswersCount
    }

    const hOnDone = (answers:number[]) => {
        setIsDone(true)
        setAnswers(answers)

        const result:QuizResult = {
          id:"", 
          quizId:quiz?.id ?? "", 
          userId:localStorage.getItem("current-user-id") as string,
          result:CalculateResult()
        }
        console.log(result)
        api.SaveResultAsync(result)
        .then((response) => {
          console.log(response)
          if(response.code == 401) {
            setErrorMessage("Authorize to be able to see and save the results!");
            setIsError(true);
          }else{
            setIsError(false);
          }
        })
    }

    const onReset = () => {
        setIsDone(false)
    }

    return (<QuizLayout>
        <div className="col-lg-4 col-md-7 col-sm-12 mx-auto">
          { isError ? <Notification message={errorMessage} title="Warning"/>
          : <div> { quiz?
                (!isDone 
                    ? <Quiz quiz={quiz} onDone={hOnDone}></Quiz> 
                    : <div>
                        <h2>Your result: {CalculateResult()}</h2>
                        <QuizResults 
                        userAnswers={userAnswers} 
                        questions={quiz.questions} 
                        onRestart={onReset}/>
                        <h5 className="text-center">Results</h5>
                        <QuizResultsList results={results}/>
                    </div>)
                : (<h1>Loading...</h1>)
            }
            </div>
          }
        </div>
    </QuizLayout>)
}
