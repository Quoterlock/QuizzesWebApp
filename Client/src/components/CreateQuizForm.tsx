import { useState, FormEvent } from "react"
import CreateQuestionForm from "./CreateQuestionForm"

interface Props {
    onSubmit:(quiz:QuizItem) => void
}


export default function CreateQuizForm() {
    console.log("Parent component rerendering...")
    const [questions, setQuestions] = useState<QuestionItem[]>([])

    const onAddNew = (question: QuestionItem) => {
        setQuestions([...questions, question])
    }

    function handleSubmit(e:FormEvent<HTMLFormElement>){
        e.preventDefault();
        const form:HTMLFormElement = e.currentTarget
        const formData = new FormData(form);
        //convert to item
        // onSubmit(quizItem)
        
    }

    function hQuestionChange(index:number, question:QuestionItem) {
        const newArray = [...questions];
        newArray[index] = question
        console.log("Change question in list")
        setQuestions(newArray)
    }


    const addNewQuestion = () => {
        setQuestions([...questions,GetDefaultQuestion()])
    }
    
    return(<div>
        <div className="d-grid form-inputs mb-3">
            <label>Title</label>
            <input type="text"/>
        </div>
        <div>
            {
                questions.map((question, index)=>(
                    <div key={index}>
                        <CreateQuestionForm key={index} question={question} onChange={(q:QuestionItem) => hQuestionChange(index, q)}/>
                    </div>
                ))
            }
        </div>
        <div className="d-grid mt-3">
        <button className="btn active-btn" onClick={addNewQuestion}>Add Question</button>
        </div>
        <div className="d-grid mt-3">
        <button className="btn active-btn" onClick={addNewQuestion}>Create Quiz</button>
        </div>
    </div>)
}

function GetDefaultQuestion():QuestionItem{
    return({correctAnswerIndex:0, options:[{text:""}]})
}