import { useState } from "react"
import Question from "./Question"

interface Props {
    quiz:QuizItem
    onDone: (answers: number[]) => void
}

export default function Quiz({quiz, onDone}:Props) {
    
    const [selectedQuestion, setSelectedQuestion] = useState(0)
    const [userAnswers, setAnswer] = useState<number[]>(new Array(quiz.questions.length))
    const [showAnswer, setShowAnswer] = useState(false)
    console.log(userAnswers)
    const hNextQuestion = () => {
        setSelectedQuestion(selectedQuestion+1)
    }
    
    const hPrevQuestion = () => {
        setSelectedQuestion(selectedQuestion-1)
    }

    const hOnAnswerChange = (index:number) => {
        setAnswer(
            (prev) => {
                const updated = [...prev];
                updated[selectedQuestion] = index;
                return updated
            })
    }

    const hShowAnswerChange = () => {
        setShowAnswer(!showAnswer)
    }

    return(
        <div>
            <h1>{"Quiz: " + quiz.title}</h1>
            <div className="block-style mb-3">
                <p className="text-center">{
                    (selectedQuestion + 1).toString() + "/" + quiz.questions.length.toString()
                }</p>
                <Question 
                    onAnswerChange={hOnAnswerChange} 
                    item={quiz.questions[selectedQuestion]} 
                    userAnswer={userAnswers[selectedQuestion]}/>
                
                { showAnswer && <div>
                    { quiz.questions[selectedQuestion].correctAnswerIndex === userAnswers[selectedQuestion] 
                    ? <p className="correct-answer-label-style text-center">Correct!</p> 
                    : <p className="wrong-answer-label-style text-center">Wrong!</p> }
                    </div>
                }    
            </div>
            <button className="btn active-btn" onClick={hPrevQuestion}>Prev</button>
            <button className="btn active-btn" onClick={hNextQuestion}>Next</button>
            <button className="btn active-btn" onClick={hShowAnswerChange}>Show Answer</button>
            <button className="btn active-btn" onClick={()=> onDone(userAnswers)}>Finish</button>
        </div>
    )
}