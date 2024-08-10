import { QuizLayout } from "../layout/QuizLayout"
import Quiz from './Quiz'
import { useContext, useState, useEffect, ReactNode } from "react"
import { useParams } from "react-router"
import { AppContext } from "../../services/AppContext"
import QuizResults from "./QuizResults"
import Notification from "../shared/Notification"
import QuizResultsList from "./QuizResultsList"
import { Button } from "../shared/Button"

export default function QuizPage(){
    const {id} = useParams()
    const [quiz, setQuiz] = useState<QuizItem>()
    const [isDone, setIsDone] = useState(false)
    const [isStarted, setIsStarted] = useState(false)
    const [userAnswers, setAnswers] = useState<number[]>([])
    const {api} = useContext(AppContext)   
    const [errorMessage, setErrorMessage] = useState("")
    const [isError, setIsError] = useState(false)

    useEffect(() => {
        const fetchData = async () => {
          try {
            const response = await api.GetByIdAsync(id as string);
            setQuiz(response);
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

    const hOnDone = (answers:number[]) => {
        setIsDone(true)
        setAnswers(answers)

        var correctAnswersCount = 0;
        quiz?.questions.map((q, index)=> {
          if(q.correctAnswerIndex === userAnswers[index])
            correctAnswersCount++
        })
        const finalResult = correctAnswersCount*100/(quiz?.questions.length??correctAnswersCount)

        const result:QuizResult = {
          id:"", 
          quizId:quiz?.id ?? "", 
          result:finalResult
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

    const hOnStart = () => {
      setIsStarted(true);
    }

    const onReset = () => {
        setIsStarted(false)
        setIsDone(false)
    }

    return (<QuizLayout>
        <div className="col-lg-4 col-md-7 col-sm-12 mx-auto">
          { isError ? <Notification message={errorMessage} title="Warning"/>
          : <div> { quiz!
            ? (!isStarted
              ? (<>
                <div className="d-grid mb-3">
                  <Button type="active" onClick={hOnStart}>Start Quiz</Button>
                </div>
                { PrintTestResults(quiz.results??[]) }
              </>)
              : (!isDone 
                ? <Quiz quiz={quiz} onDone={hOnDone}></Quiz> 
                : <div>
                    <QuizResults 
                    userAnswers={userAnswers} 
                    questions={quiz.questions} 
                    onRestart={onReset}/>
                    { PrintTestResults(quiz.results??[]) }
                </div>
                )
            )
                
                : (<h1>Loading...</h1>)
            }
            </div>
          }
        </div>
    </QuizLayout>)
}

function PrintTestResults(results:QuizResult[]):ReactNode {
    return(<>
      <h5 className="text-center">User Results</h5>
      <QuizResultsList results={results}/>
    </>)
}
