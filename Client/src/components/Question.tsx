import { useState } from "react"

interface Props {
    onAnswerChange: (index:number) => void
    item: QuizItem
    userAnswer: number
}

export default function Question({item, onAnswerChange, userAnswer}:Props) {
    const [selectedAnswer, setSelectedAnswer] = useState(0)
    console.log(selectedAnswer)
    const hOnClick = (index: number) => {
        setSelectedAnswer(index)
        onAnswerChange(index)
    }

    return(
        <div>
            <h5 className="text-center">{item.question}</h5>
            { item.answers.map((answer, index) => (
                <div onClick={() => hOnClick(index)}
                className={(index === selectedAnswer ? "selected-answer-style" : "answer-style") + " mb-2"}
                >{answer}</div>
                
            ))}
        </div>
    )
}