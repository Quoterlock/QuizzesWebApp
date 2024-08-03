import { useState, ChangeEvent } from "react"
import CreateQuestionForm from "./CreateQuestionForm"
import Notification from "../../shared/Notification"

interface Props {
    onCreate:(quiz:QuizItem) => void
}


export default function CreateQuizForm({onCreate}:Props) {
    const [questions, setQuestions] = useState<QuestionItem[]>([])
    const [title, setTitle] = useState("")
    const [isNotify, setIsNotify] = useState(false);
    
    const hCreateQuiz = () => {
        const quiz: QuizItem = {
            id:"",
            questions:[...questions],
            rate:0,
            title:title,
            author:"",
            authorId:"",
            creationDate:"",
            results:[]
        }

        if(ValidateQuiz(quiz)){
            console.log("Quiz is valid")
            onCreate(quiz)
        } else {
            console.log("Quiz is not valid")
            setIsNotify(true)
        }
    }

    const hQuestionChange = (index:number, question:QuestionItem) =>  {
        const newArray = [...questions];
        newArray[index] = question
        setQuestions(newArray)
        setIsNotify(false)
    }

    const hOnRemove = (index:number) => {
        setQuestions(questions
            .filter((item, curIndex)=>curIndex !== index)
        )
    }


    const addNewQuestion = () => {
        setQuestions([...questions,GetDefaultQuestion()])
    }
    
    return(<div>
        {
            isNotify && 
            <div className="mb-3">
                <Notification title="Submit error!" message="Please, fill all required fields"/>
            </div>
        }
        <div className="d-grid form-inputs mb-3 block-style">
            <label className="mb-2">Title</label>
            <div className="edit-option-style">
                <input className={(StringIsEmptyOrNull(title)?"wrong-input":"") + " option-textbox"} type="text" value={title} onChange={(e:ChangeEvent<HTMLInputElement>)=>setTitle(e.currentTarget.value)}/>
                <button className="btn active-btn ms-2" onClick={()=>hCreateQuiz()}>Create</button>
            </div>
        </div>
        <div>
            {
                questions.map((question, index)=>(
                    <div key={index}>
                        <CreateQuestionForm key={index} question={question} onChange={(q:QuestionItem) => hQuestionChange(index, q)} onRemove={()=>hOnRemove(index)}/>
                    </div>
                ))
            }
        </div>
        <div className="d-grid mt-3">
            <button className="btn active-btn" onClick={addNewQuestion}>Add Question</button>
        </div>
    </div>)
}

function GetDefaultQuestion():QuestionItem{
    return({correctAnswerIndex:0, options:[{text:""}], text:""})
}

function StringIsEmptyOrNull(value:string): boolean {
    return (value === null || value === "")
}

function ValidateQuiz(quiz:QuizItem):boolean {
    if(StringIsEmptyOrNull(quiz.title)) {
        return false
    }
    if(quiz.questions.length > 0) {
        for(let i = 0; i < quiz.questions.length; i++) {
            if(!StringIsEmptyOrNull(quiz.questions[i].text) && quiz.questions[i].options.length > 0) {
                for(let j = 0; j < quiz.questions[i].options.length; j++) {
                    if(StringIsEmptyOrNull(quiz.questions[i].options[j].text)){
                        return false
                    }
                }
            } else return false
        }
    } else return false
    
    return true
}