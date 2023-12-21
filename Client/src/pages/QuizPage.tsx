import { QuizLayout } from "../layouts/QuizLayout"
import Quiz from '../components/Quiz'
import { useState } from "react"
import { GetQuizById } from '../models/apiManager'
import { useParams } from "react-router"

export default function QuizPage(){
    const {id} = useParams()
    const [quiz, setQuiz] = useState<QuizItem>()
    
    GetQuizById(id as string, setQuiz)
    
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
