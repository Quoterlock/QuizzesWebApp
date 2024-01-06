import { QuizLayout } from "../layouts/QuizLayout"
import Quiz from '../components/Quiz'
import { useContext, useState, useEffect } from "react"
import { useParams } from "react-router"
import { AppContext } from "../services/app-context"
import QuizResults from "../components/QuizResults"

export default function QuizPage(){
    const {id} = useParams()
    const [quiz, setQuiz] = useState<QuizItem>()
    const {api} = useContext(AppContext)   
    
    useEffect(() => {
        const fetchData = async () => {
          try {
            const response = await api.GetByIdAsync(id as string);
            setQuiz(response);
            console.log(response);
          } catch (error) {
            // Handle errors
            console.error(error);
          }
        };
    
        fetchData();
      }, []);

    const [isDone, setIsDone] = useState(false)
    const [userAnswers, setAnswers] = useState<number[]>([])

    const hOnDone = (answers:number[]) => {
        setIsDone(true)
        setAnswers(answers)
    }

    const onReset = () => {
        setIsDone(false)
    }

    return (<QuizLayout>
        <div className="col-lg-4 col-md-7 col-sm-12 mx-auto">
        { quiz 
            ? (!isDone 
                ? <Quiz quiz={quiz} onDone={hOnDone}></Quiz> 
                : <QuizResults userAnswers={userAnswers} questions={quiz.questions} onRestart={onReset}/>)
            : (<h1>Loading...</h1>)
        }
        </div>
    </QuizLayout>)
}
