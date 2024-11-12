import axios from "axios";
import { env } from "../../../env/env.dev";
import { endpoints } from "../config/apiConfig";

interface GetCommentsByPostIdRequestRsDto {
  comments: any,
}

export const getCommentsByPostIdRequest = async (postId: number): Promise<GetCommentsByPostIdRequestRsDto> => {
  try {
    const response = await axios.get(`${env.baseUrl}${endpoints.getCommentsByPostId(postId)}`);
    // console.log(response);
    return response.data;
  } catch (error) {
    if (axios.isAxiosError(error)) {
      console.error('Błąd Axios:', {
          message: error.message,
          status: error.response?.status,
          data: error.response?.data,
      });
    } else {
      console.error('Inny błąd:', error);
    }
    console.error('Error getCommentsByPostIdRequest:', error);
    return [] as unknown as GetCommentsByPostIdRequestRsDto;
  }
};