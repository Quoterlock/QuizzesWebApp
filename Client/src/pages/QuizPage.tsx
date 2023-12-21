import { QuizLayout } from "../layouts/QuizLayout"
import Quiz from '../components/Quiz'
import { useState } from "react"
import { GetQuizById } from '../models/apiManager'
import { useParams } from "react-router"

export default function QuizPage(){

    const [quiz, setQuiz] = useState(InitQuestions())
    const [isDone, setIsDone] = useState(false)
    const [userAnswers, setAnswers] = useState<number[]>(new Array(quiz.questions.length, 0))
    const {id} = useParams()

    setQuiz(GetQuizById(id as string))

    const hOnDone = (answers:number[]) => {
        setIsDone(true)
        setAnswers(answers)
    }

    const onReset = () => {
        setQuiz(InitQuestions())
        setIsDone(false)

    }

    return (<QuizLayout>
        <div className="col-4 mx-auto">
        { !isDone ? <Quiz quiz={quiz} onDone={hOnDone}></Quiz> :
            <div>
                <button className="btn active-btn mb-3" onClick={onReset}>Restart</button>
                { quiz.questions.map((question, index) => (
                <div key={index} className="block-style mb-3">
                    <h5>{(index+1 ).toString() + ". " + question.question}</h5>
                    <p className={ question.correctAnswerIndex === userAnswers[index] 
                    ? "correct-answer-label-style" 
                    : "wrong-answer-label-style"}>{"Your answer: " + question.answers[userAnswers[index]]}</p>
                    <p>{"Correct answer: " + question.answers[question.correctAnswerIndex]}</p>
                </div>
            ))}
            </div>
        }
        </div>
    </QuizLayout>)
}

function InitQuestions(): QuizItem {
    const arr: QuestionItem[] = [
        {
            id: "1",
            question: "Як перекладається слово Wizard?",
            answers: [
                "а) Чарівник","б) Метелиця","в) Привид"
            ],
            correctAnswerIndex:0
        },
        {
            id: "2",
            question: "Як перекладається слово Adventure?",
            answers: [
                "а) Робота","б) Розвиток", "в) Пригода"
            ],
            correctAnswerIndex:2
        }
    ]

    return ({rate:5, questions:arr, author:"Dick Dickkenson", title:"English words"})
}
