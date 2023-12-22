import { QuizLayout } from "../layouts/QuizLayout"
import Quiz from '../components/Quiz'
import { useContext, useState } from "react"
import { useParams } from "react-router"
import { AppContext } from "../services/app-context"

export default function QuizPage(){
    const {id} = useParams()
    const [quiz, setQuiz] = useState<QuizItem>()
    const {api} = useContext(AppContext)   
    api.GetById(id as string, setQuiz);

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
        <div className="col-4 mx-auto">
        { quiz ? (
            !isDone ? <Quiz quiz={quiz} onDone={hOnDone}></Quiz> :
                <div>
                    <button className="btn active-btn mb-3" onClick={onReset}>Restart</button>
                    { quiz.questions.map((question, index) => (
                    <div key={index} className="block-style mb-3">
                        <p className={ question.correctAnswerIndex === userAnswers[index] 
                        ? "correct-answer-label-style" 
                        : "wrong-answer-label-style"}>{"Your answer: " + question.options[userAnswers[index]].text}</p>
                        <p>{"Correct answer: " + question.options[question.correctAnswerIndex].text}</p>
                    </div>
                ))}
                </div>
            ) : (<h1>Loading...</h1>)
        }
        </div>
    </QuizLayout>)
}
