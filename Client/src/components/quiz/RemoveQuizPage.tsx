import { useNavigate, useParams } from "react-router"
import { Button } from "../shared/Button"
import { useContext } from "react"
import { AppContext } from "../../services/AppContext"

export function RemoveQuizPage() {
   
   const {id} = useParams()
   const {api} = useContext(AppContext)
   const navigate = useNavigate()
   const onConfirm = () => {
      api.DeleteQuiz(id as string)
         .catch((error)=>{
            console.log(error.message) 
            navigate("/")
         })
         .then((result)=>{
            console.log(result)
            navigate("/")
         })
   }
   const onCancel = () => {
      navigate("/")
   }

   return(<div className="col-lg-4 col-md-7 col-sm-12 mx-auto block-style">
        <h5 className="text-center">Delete confirmation</h5>
        <p>Are you sure you want to delete this quiz?</p>
        
        <Button type="danger" onClick={onConfirm}>Confirm</Button>
        <Button type="minor" onClick={onCancel}>Cansel</Button>
   </div>)
}