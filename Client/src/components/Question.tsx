import { useState } from "react"

interface Props {
    onAnswerChange: (index:number) => void
    item: QuizItem
}

export default function Question({item, onAnswerChange}:Props) {
    const [selectedAnswer, setSelectedAnswer] = useState(0)
    
    const hOnClick = (index: number) => {
        setSelectedAnswer(index)
        onAnswerChange(index)
    }

    return(
        <div className="block-style">
            <h4>{item.question}</h4>
            { item.answers.map((answer, index) => (
                <div onClick={() => hOnClick(index)}
                className={index === selectedAnswer ? "selected-answer-style" : "answer-style"}
                >{answer}</div>
                
            ))}
        </div>
    )
}